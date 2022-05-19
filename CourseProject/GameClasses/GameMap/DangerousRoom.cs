using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using GameClasses.Beings;
using GameClasses.Factory.BeingsFactory;
using GameClasses.Factory.StuffFactory;
using GameClasses.Weapons;
using GameClasses.GameObjects;

namespace GameClasses.GameMap
{
    public class DangerousRoom : Room
    {
        public Room Room { get; }
        public List<Enemy> Enemies { get; protected set; }
        public DangerousRoom(Room room) : base(room.Place, room.Colider.Width, room.Colider.Height)
        {
            Enemies = new List<Enemy>();
            Room = room;
        }

        public override void PlaceComponentsOnMap()
        {
            List<IGenerable> listOfStuff = GetStuff();
            List<Enemy> enemies = new List<Enemy>();
            Thread.Sleep(10);
            int numberOfEnemies = random.Next(4,6);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                IWeapon tempWeapon = new WeaponFactory().CreateStuff(Point.Empty);
                enemies.Add(new BeingFactory().Create(Room,tempWeapon,Point.Empty));
            }
            listOfStuff.AddRange(enemies);
            SetComponentsOnMap(listOfStuff);
            Stuffs.RemoveAll(st=>st is Enemy);
            Enemies.AddRange(enemies);
        }
    }
}
