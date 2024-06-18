using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanic
{
    public static class ResourceManager
    {
        public static float ScrollSpeed { get { return 10; } }
        public const float ScrollSpeed2 = 10;
        public static float RotateSpeed { get { return 10; } }
        public static int ScrollWidth { get { return 15; } }
        public static float MinCameraHeight { get { return 10; } }
        public static float MaxCameraHeight { get { return 40; } }
        public static float RotateAmount { get { return 100; } }

        public enum Direction { left, right };
					

        /// <summary>
        /// typy części
        /// </summary>
	
		public static Vector3 CameraInitialPosition{
		 	get {

					return new Vector3(14.11857f, 9.629429f, -2f);  
			}
		}
		public static Vector3 CameraInitialRotation{
			get {
				
				return new Vector3(10, 270, 0);  
			}
		}
        public enum PartTypes
        {
            NullPartType,
            TrippleSemiConductorWithFazeShifter,
            Radiator,
            MainBoard

            /*nullPartType, //na przyszłość
            //typy statków
            XFigther,
            //typy silników
            MonoJet,
            TripleJet,
            TurboJet,
            //typy gaźników
            DoubleCarburetor*/
        }

        /// <summary>
        /// sklepowe kategorie części
        /// </summary>
        public enum Categories
        {
            nullCategory, //na przyszłość
            spaceShip,
            engine,
            filter,
			bolt
        }

        /// <summary>
        /// w przyszlości znajdą się tutaj wszystkie game objecty części - podpinane konstrukcją podobna do player = transform.root.GetComponent<Player>();
        /// nie wiem na tą chwilę gdzie będzie najlepsze miejsce na inicjalizacje.
        /// planuje zrobić managera wyciągającego party po kategori i inne przydatne medoty sprawdzające gdzie część można podpiąć itd.
        /// </summary>
        public static ICollection<BasePart> ProtoParts;
    }
}