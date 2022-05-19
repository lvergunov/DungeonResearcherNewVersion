using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameClasses.Enums;
using GameClasses.Game;
using GameClasses.GameMap;
using GameClasses.Weapons;

namespace GameClasses.Beings
{
    public class Hero : Being
    {
        public delegate void HerosActions();
        public event HerosActions onRelocation;
        public delegate void TakeWeapon(IWeapon weapon);
        public event TakeWeapon onWeaponTook;
        public Hero(Room spawnRoom, float minDamage, float maxDamage, Point position, int coliderWidth, int coliderHeight, string name, IWeapon primaryWeapon, 
            float speedPerMS, float health) :
            base(spawnRoom, primaryWeapon, minDamage, maxDamage, position, coliderWidth,
                coliderHeight, name, speedPerMS, health)
        {
            MaximumWeapons = 2;
        }

        public override bool TryGetNearestObject(int distance, out IObjectOnGameMap nearestObject)
        {
            try
            {
                Rectangle pathColider = GetPathColider(distance);
                List<IObjectOnGameMap> objects = new List<IObjectOnGameMap>();
                var selectedItems = from item in NativeRoom.Stuffs
                                    where item.Colider.IntersectsWith(pathColider) && item.IsSolid
                                    select item;
                objects.AddRange(selectedItems);
                if (NativeRoom is DangerousRoom dangerous)
                {
                    var selectedEnemies = from enemy in dangerous.Enemies
                                          where enemy.Colider.IntersectsWith(pathColider) && enemy.IsSolid
                                          select enemy;
                    objects.AddRange(selectedEnemies);
                }
                if (objects.Count == 0)
                {
                    nearestObject = null;
                    return false;
                }
                switch (Direction)
                {
                    case Directions.Left:
                        var sorted = objects.OrderByDescending(p => p.Colider.Left);
                        nearestObject = sorted.FirstOrDefault();
                        break;
                    case Directions.Right:
                        sorted = objects.OrderBy(p => p.Colider.Left);
                        nearestObject = sorted.FirstOrDefault();
                        break;
                    case Directions.Up:
                        sorted = objects.OrderByDescending(p => p.Colider.Top);
                        nearestObject = sorted.FirstOrDefault();
                        break;
                    case Directions.Down:
                        sorted = objects.OrderBy(p => p.Colider.Top);
                        nearestObject = sorted.FirstOrDefault();
                        break;
                    default:
                        throw new Exception();
                }
                return true;
            }
            catch {
                return TryGetNearestObject(distance,out nearestObject);
            }
        }
        public void Relocate(Room room, RoomExit exit)
        {
            NativeRoom = room;
            Point controlPoint = Colider.Location;
            switch (exit.SideOfRoom)
            {
                case Directions.Down:
                    controlPoint = new Point(exit.Colider.Left + exit.Colider.Width / 2,
                        exit.Colider.Bottom - ColliderHeight - 5);
                    break;
                case Directions.Up:
                    controlPoint = new Point(exit.Colider.Left + exit.Colider.Width / 2,
                        exit.Colider.Top + ColliderHeight + 5);
                    break;
                case Directions.Left:
                    controlPoint = new Point(exit.Colider.Left +
                        ColliderWidth + 5,
                        exit.Colider.Top + exit.Colider.Height / 2);
                    break;
                case Directions.Right:
                    controlPoint = new Point(exit.Colider.Right - ColliderWidth - 5,
                        exit.Colider.Top + exit.Colider.Height / 2);
                    break;
            }
            Place = controlPoint;
            SinchronizeWeapon();
            if (NativeRoom is DangerousRoom)
            {
                Thread.Sleep(500);
                onRelocation();
            }
        }
        public void ChangeWeapon()
        {
            IWeapon previous = Arsenal[0];
            Arsenal.RemoveAt(0);
            Arsenal.Add(previous);
            SinchronizeWeapon();
        }
        public void ChangeRoom(Room newRoom)
        {
            NativeRoom = newRoom;
        }

        public void RestoreHealth(float healthRate)
        {
            if (Health + healthRate > FullHealth)
                Health = FullHealth;
            else Health += healthRate;
        }

        public void TakeNewWeapon(IWeapon newWeapon)
        {
            if (MaximumWeapons > Arsenal.Count)
                Arsenal.Add(newWeapon);
            else {
                Arsenal.RemoveAt(0);
                Arsenal.Insert(0,newWeapon);
            }
            SinchronizeWeapon();
            onWeaponTook(newWeapon);
        }
    }
}
