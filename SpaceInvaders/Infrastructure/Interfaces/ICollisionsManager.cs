using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    // TODO 06: Define the base interface for collidable objects (2D/3D):
    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;

        event EventHandler<EventArgs> SizeChanged;

        event EventHandler<EventArgs> VisibleChanged;

        event EventHandler<EventArgs> Disposed;

        bool Visible { get; }
        bool CheckCollision(ICollidable i_Source);
        void Collided(ICollidable i_Collidable);
    }

    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }
        Vector2 Velocity { get; }
    }

    public interface ICollidable3D : ICollidable
    {
        BoundingBox Bounds { get; }
        Vector3 Velocity { get; }
    }

    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
}
