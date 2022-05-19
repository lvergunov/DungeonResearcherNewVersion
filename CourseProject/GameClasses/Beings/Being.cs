using GameClasses.Enums;
using GameClasses.Game;
using GameClasses.GameMap;
using GameClasses.GameObjects;
using GameClasses.Weapons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace GameClasses.Beings
{
    public abstract class Being : IGenerable,IDestroyable
    {
        public delegate void PunchDelegate();

        public event PunchDelegate beingIsDead;
        
        public int Level { get;protected set; }
        
        public RarityOfStuff Rarity { get; protected set; }

        public bool IsVisible { get; protected set; }
        
        public bool IsSolid { get { return IsAlive; } }

        public bool IsDestroyable { get { return true; } }

        public void Show()
        {
            IsVisible = true;
        }
        public void Hide()
        {
            IsVisible = false;
        }
        public List<IWeapon> Arsenal { get; protected set; }
        public IWeapon WeaponInHands { get { return Arsenal[0]; } }
        public Directions Direction { get; protected set; }
        public virtual float MaximalDamage { get; }
        public virtual float MinimalDamage { get; }
        public virtual int MaximumWeapons { get; protected set; }

        public int AbsoluteWidth { get; }

        public int AbsoluteHeight { get; }

        public virtual void Punch()
        {
            try { 
                WeaponInHands.Use(this); 
            }
            catch { }
        }
        public Being(Room spawnRoom,IWeapon weapon,float minDamage,
            float maxDamage,Point position,int coliderWidth,
            int coliderHeight,string name,float speed,float health)
        {
            Arsenal = new List<IWeapon>();
            Arsenal.Add(weapon);
            MinimalDamage = minDamage;
            MaximalDamage = maxDamage;
            Name = name;
            NativeRoom = spawnRoom;
            Place = position;
            ColliderWidth = coliderWidth;
            ColliderHeight = coliderHeight;
            Direction = Directions.Up;
            SpeedPerMS = speed;
            FullHealth = health;
            IsAlive = true;
            AbsoluteWidth = coliderWidth;
            AbsoluteHeight = coliderHeight;
            Health = health;
            SinchronizeWeapon();
        }
        public Room NativeRoom { get; protected set; }
        public Rectangle Colider { get {
                return new Rectangle(Place,new Size(ColliderWidth,ColliderHeight));
            } }
        public Point Place { get; protected set; }
        public string Name { get; }
        public int ColliderWidth { get; protected set; }
        public int ColliderHeight { get; protected set; }
        public void GetPunched(float damage,Being sender)
        {
            Health -= damage;
            Move(sender.Direction,(int)SpeedPerMS*2);
            if (Health <= 0)
            {
                IsAlive = false;
                beingIsDead();
            }
        }

        public void GetPunched(float damage) {
            Health -= damage;
            Move(DirectionOperatons.GetOpposite(Direction), (int)SpeedPerMS * 2);
            if (Health <= 0)
            {
                IsAlive = false;
                beingIsDead();
            }
        }
        public virtual float FullHealth { get; }

        public float Health { get; protected set; }

        public virtual float SpeedPerMS { get; }
        public void ChangePosition(Point point)
        {
            Place = point;
            SinchronizeWeapon();
        }
        public async virtual void Move(Directions direction, int totalDistance) {
                for (int i = 0; i < totalDistance; i++)
                {
                    if (direction == Direction)
                    {
                        Point trialPoint = GetPointByDirection(direction, 1);
                        Rectangle trialColider = new Rectangle(trialPoint, new Size(ColliderWidth, ColliderHeight));
                        if (trialColider.Left < NativeRoom.Colider.Left || trialColider.Right > NativeRoom.Colider.Right ||
                            trialColider.Top < NativeRoom.Colider.Top || trialColider.Bottom > NativeRoom.Colider.Bottom)
                            trialPoint = GetIntermediateInRoom(direction, NativeRoom.Colider);
                        else if (TryGetNearestObject(1, out IObjectOnGameMap nearest))
                        {
                            trialPoint = GetIntermediateWithObject(Direction, nearest.Colider);
                            break;
                        }
                        Place = trialPoint;
                    }
                    else
                        Turn(direction);
                    SinchronizeWeapon();
                }
        }
        public void Turn(Directions direction) {
            if (Direction == Directions.Left || Direction == Directions.Right)
            {
                if (direction == Directions.Right || direction == Directions.Left)
                    Direction = direction;
                else if (direction == Directions.Up || direction == Directions.Down
                     && Colider.Left + Colider.Width / 2 - Colider.Height / 2 >= NativeRoom.Colider.Left &&
                    Colider.Left + Colider.Width / 2 - Colider.Height / 2 <= NativeRoom.Colider.Right) {
                    Place = new Point(Colider.Left+Colider.Width/2 - 
                        Colider.Height/2,Colider.Top+Colider.Height/2-Colider.Width/2);
                    int tempInt = ColliderWidth;
                    ColliderWidth = ColliderHeight;
                    ColliderHeight = tempInt;
                    Direction = direction;
                }
            }
            else if (Direction == Directions.Up || Direction == Directions.Down)
            {
                if (direction == Directions.Down || direction == Directions.Up)
                    Direction = direction;
                else if(direction == Directions.Left || direction == Directions.Right &&
                    Colider.Top+Colider.Height/2-Colider.Width/2>=NativeRoom.Colider.Top && 
                    Colider.Top+Colider.Height/2+Colider.Width/2<=NativeRoom.Colider.Bottom)
                {
                    Place = new Point(Colider.Left + Colider.Width/2-Colider.Height/2,
                        Colider.Top+Colider.Height/2-Colider.Width/2);
                    int tempInt = ColliderWidth;
                    ColliderWidth = ColliderHeight;
                    ColliderHeight = tempInt;
                    Direction = direction;
                }
            }
        }
        public bool IsAlive { get; protected set; }
        protected Being(Room spawnRoom, List<IWeapon> weapons, float minDamage,
            float maxDamage, Point position, int coliderWidth,
            int coliderHeight, string name, float speed, float health) : this(spawnRoom,
                weapons[0],minDamage,minDamage,position,coliderWidth,coliderHeight,
                name,speed,health) {
            for (int i = 1; i < weapons.Count; i++)
                Arsenal.Add(weapons[i]);
        }
        protected virtual Point GetIntermediateInRoom(Directions direction, Rectangle roomColider)
        {
            switch (direction)
            {
                case Directions.Up:
                    return new Point(Colider.Left,roomColider.Top);
                case Directions.Down:
                    return new Point(Colider.Left,roomColider.Bottom - Colider.Height);
                case Directions.Left:
                    return new Point(roomColider.Left,Colider.Top);
                case Directions.Right:
                    return new Point(roomColider.Right-Colider.Width,Colider.Top);
                default:
                    throw new Exception();
            }
        }
        protected virtual Point GetIntermediateWithObject(Directions direction,Rectangle objectColider)
        {
            switch (direction)
            {
                case Directions.Up:
                    return new Point(Colider.Left,objectColider.Bottom);
                case Directions.Down:
                    return new Point(Colider.Left, objectColider.Top-Colider.Height);
                case Directions.Left:
                    return new Point(objectColider.Right,Colider.Top);
                case Directions.Right:
                    return new Point(objectColider.Left-Colider.Width,Colider.Top);
                default:
                    throw new Exception();
            }
        }
        protected Point GetPointByDirection(Directions direction,int distance)
        {
            Point trialPosition = this.Place;
            switch (direction)
            {
                case Directions.Up:
                    trialPosition = new Point(this.Place.X, this.Place.Y - distance);
                    break;
                case Directions.Down:
                    trialPosition = new Point(this.Place.X, this.Place.Y + distance);
                    break;
                case Directions.Left:
                    trialPosition = new Point(this.Place.X - distance, this.Place.Y);
                    break;
                case Directions.Right:
                    trialPosition = new Point(this.Place.X + distance, this.Place.Y);
                    break;
            }
            return trialPosition;
        }
        public int FindEuclideanDistance(IObjectOnGameMap gameObject)
        {
            Point thisObjectCenter = new Point(Colider.Left + Colider.Width, Colider.Top + Colider.Height);
            Point innerObjectCenter = new Point(gameObject.Colider.Left +
                gameObject.Colider.Width, gameObject.Colider.Top + gameObject.Colider.Height);
            return (int)Math.Sqrt(Math.Pow(thisObjectCenter.X - innerObjectCenter.X, 2) +
                Math.Pow(thisObjectCenter.Y - innerObjectCenter.Y, 2));
        }

        public Rectangle GetPathColider(int distance)
        {
            Rectangle damagingColider = new Rectangle();
            switch (Direction)
            {
                case Directions.Up:
                    damagingColider = new Rectangle(Colider.Left, Colider.Top
                        - distance, Colider.Width,
                        distance);
                    break;
                case Directions.Right:
                    damagingColider = new Rectangle(Colider.Right, Colider.Top,
                        distance, Colider.Height);
                    break;
                case Directions.Left:
                    damagingColider = new Rectangle(Colider.Left - distance,
                        Colider.Top, distance, Colider.Height);
                    break;
                case Directions.Down:
                    damagingColider = new Rectangle(Colider.Left, Colider.Bottom,
                        Colider.Width, distance);
                    break;
            }
            return damagingColider;
        }
        public abstract bool TryGetNearestObject(int distance, out IObjectOnGameMap nearestObject); 
        protected void SinchronizeWeapon() {
            switch (Direction)
            {
                case Directions.Left:
                    WeaponInHands.ChangeOrientation(Orientation.Horizontal);
                    WeaponInHands.ChangePosition(new Point(Colider.Left-3*WeaponInHands.Colider.Width/4,
                        Colider.Top+Colider.Height/2-WeaponInHands.Colider.Height));
                    break;
                case Directions.Right:
                    WeaponInHands.ChangeOrientation(Orientation.Horizontal);
                    WeaponInHands.ChangePosition(new Point(Colider.Right-WeaponInHands.Colider.Width/4,
                        Colider.Top + Colider.Height / 2 + WeaponInHands.Colider.Height));
                    break;
                case Directions.Up:
                    WeaponInHands.ChangeOrientation(Orientation.Vertical);
                    WeaponInHands.ChangePosition(new Point(Colider.Left+Colider.Width/2+WeaponInHands.Colider.Width,
                        Colider.Top - 3 * WeaponInHands.Colider.Height / 4));
                    break;
                case Directions.Down:
                    WeaponInHands.ChangeOrientation(Orientation.Vertical);
                    WeaponInHands.ChangePosition(new Point(Colider.Left + Colider.Width / 2 - WeaponInHands.Colider.Width,
                        Colider.Bottom-WeaponInHands.Colider.Height/4));
                    break;
            }
            WeaponInHands.SinchroniseDamageRadius(NativeRoom, Direction);
        }
    }
}
