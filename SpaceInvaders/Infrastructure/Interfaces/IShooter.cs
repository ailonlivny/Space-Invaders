using System.Collections.Generic;
using Invaders;

namespace Infrastructure
{
    public interface IShooter
    {
        List<Bullet> Bullets { get; set; }
        bool IsShootableActive { get; set; }
        void Shoot();
    }
}
