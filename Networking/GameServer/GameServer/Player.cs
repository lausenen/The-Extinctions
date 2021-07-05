using System.Numerics;

namespace GameServer
{
    public class Player
    {
        public int id;
        public string username;

        public Vector3 position;
        public Quaternion rotation;

        public Player(int id, string username, Vector3 position)
        {
            this.id = id;
            this.username = username;
            this.position = position;
        }
    }
}