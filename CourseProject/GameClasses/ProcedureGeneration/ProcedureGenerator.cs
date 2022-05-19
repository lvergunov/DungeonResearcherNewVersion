using System.Drawing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameClasses.GameObjects;
using GameClasses.Enums;
using GameClasses.GameMap;
using GameClasses.Factory.StuffFactory;
using GameClasses.Factory.BeingsFactory;

namespace GameClasses.ProcedureGeneration
{
    public class ProcedureGenerator
    {
        public RectangularGameMap GameMap { get;  }
        public Size MaxRoom { get; }
        public Size MinRoom { get; }
        public ProcedureGenerator(RectangularGameMap gameMap,int minRoomWith,int minRoomHeight,int maxRoomWidth,int maxRoomHeight)
        {
            if ((float)gameMap.Form.Width / maxRoomWidth < 4 || (float)gameMap.Form.Height / maxRoomHeight < 4)
                throw new ArgumentException("Too big params for this map");
            MaxRoom = new Size(maxRoomWidth,maxRoomHeight);
            MinRoom = new Size(minRoomWith, minRoomHeight);
            GameMap = gameMap;
        }
        public virtual void CreateLevel(out Room heroSpawnRoom)
        {
            List<Room> Rooms = new List<Room>();
            foreach (Leaf leaf in RectangularGameMap.Leaves)
            {
                if (leaf.InnerInsertion!=null && leaf.InnerInsertion is Room innerRoom)
                    Rooms.Add(innerRoom);
            }
            int numberOfDangerousRoom = 7 * Rooms.Count / 10;
            Rooms.Sort((roomOne,roomTwo)=>(roomOne.Colider.Width*
                                roomOne.Colider.Height).CompareTo(roomTwo.Colider.Width *
                                roomTwo.Colider.Height));
            Rooms.Reverse();
            List<Room> roomsToMakeDangerous = Rooms.Take(numberOfDangerousRoom).ToList();
            List<DangerousRoom> dangerousRooms = new List<DangerousRoom>();
            foreach (Room r in roomsToMakeDangerous)
            {
                DangerousRoom newDangerousRoom = new DangerousRoom(r);
                Task.Delay(100);
                dangerousRooms.Add(newDangerousRoom);
                Rooms.Remove(r);
            }
            int spawnHeroRoomIndex = _random.Next(0,Rooms.Count);
            heroSpawnRoom = Rooms[spawnHeroRoomIndex];
            Rooms.AddRange(dangerousRooms);
            GameMap.Insertions.AddRange(Rooms);
            ConnectAllRooms(Rooms);
            Dictionary<Room, int> distancesBetweenHeroAndRooms = new Dictionary<Room, int>();
            foreach (Room r in Rooms)
                distancesBetweenHeroAndRooms.Add(r,r.FindEuclideanDistance(heroSpawnRoom));
            Room bestRoomForPortal = distancesBetweenHeroAndRooms.FirstOrDefault(x => 
                                        x.Value == distancesBetweenHeroAndRooms.Values.Max()).Key;
            Point levelExitPoint = new Point(bestRoomForPortal.Colider.Left+
                bestRoomForPortal.Colider.Width/2,bestRoomForPortal.Colider.Top+bestRoomForPortal.Colider.Height/2);
            GameMap.Add(new LevelExit(levelExitPoint,bestRoomForPortal));
            Rooms.ForEach(r=>r.PlaceComponentsOnMap());
        }

        protected void ConnectAllRooms(List<Room> Rooms) {
            int numberOfFirstRoom = _random.Next(0, Rooms.Count);
            int maxGeneration = GetMaxGeneration();
            while (maxGeneration >= 0)
            {
                var leavesLinq = from leaf in RectangularGameMap.Leaves
                                 where leaf.NodeGeneration == maxGeneration
                                 select leaf;
                List<Leaf> leaves = leavesLinq.ToList();
                foreach (Leaf leaf in leaves)
                {
                    if (leaf.ParentalLeaf is Leaf || leaf.ParentalLeaf is RectangularGameMap)
                    {
                        RectangularGameMap parent = leaf.ParentalLeaf;
                        Dictionary<Leaf, Insertion> firstGroup = GetRooms(parent.ChildNodeOne,new Dictionary<Leaf, Insertion>());
                        Dictionary<Leaf, Insertion> secondGroup = GetRooms(parent.ChildNodeTwo, new Dictionary<Leaf, Insertion>());
                        Orientation? orientation;
                            (Insertion, Insertion) tuple = FindBestSibling(firstGroup, secondGroup, out orientation);
                            RoomExit roomExitFirst = new RoomExit(tuple.Item1 as Room,tuple.Item2 as Room);
                            RoomExit roomExitSecond = new RoomExit(tuple.Item2 as Room,tuple.Item1 as Room);
                            if (!GameMap.Coridors.Contains(roomExitFirst) && !GameMap.Coridors.Contains(roomExitSecond)) {
                                GameMap.Add(roomExitFirst); GameMap.Add(roomExitSecond);
                            }
                    }
                 }
                maxGeneration--;
            }
        }
        protected Dictionary<Leaf,Insertion> GetRooms(Leaf leaf,Dictionary<Leaf,Insertion> insertions)
        {
            if (leaf.ChildNodeOne == null && leaf.ChildNodeTwo == null && leaf.InnerInsertion != null)
            {
                var searchedRoom = GameMap.Insertions.First(r=>r==leaf.InnerInsertion || 
                    r is DangerousRoom dr && dr.Room==leaf.InnerInsertion);
                insertions.Add(leaf, searchedRoom);
            }
            else
            {
                GetRooms(leaf.ChildNodeOne, insertions);
                GetRooms(leaf.ChildNodeTwo, insertions);
            }
            return insertions;
        }
        protected (Insertion,Insertion) FindBestSibling(Dictionary<Leaf,Insertion> firstGroup,
            Dictionary<Leaf,Insertion> secondGroup,out Orientation? orientation) {
            orientation = null;
            Orientation? tempOrientation;
            int totalSide = Int32.MaxValue;
            Insertion roomOne = null;
            Insertion roomTwo = null;
            foreach (KeyValuePair<Leaf,Insertion> r1 in firstGroup) {
                foreach (KeyValuePair<Leaf, Insertion> r2 in secondGroup) {
                    tempOrientation = RoomExit.CheckRelations(r1.Key.Form, r2.Key.Form);
                    int tempDistance = r1.Value.FindEuclideanDistance(r2.Value);
                    if (tempOrientation != null && tempDistance<totalSide)
                    {
                        orientation = tempOrientation;
                        roomOne = r1.Value; roomTwo = r2.Value; totalSide = tempDistance;
                    }
                }
            }

            if (roomOne == null || roomTwo == null || orientation==null)
                throw new Exception("There is not fitable pair");
            return (roomOne,roomTwo);
        }
        protected int GetMaxGeneration()
        {
            var lastGenLinq = RectangularGameMap.Leaves.Max(c => c.NodeGeneration);
            return lastGenLinq;
        }
        private Random _random = new Random();
    }
}
