using FlockBuddy.Interfaces;
using FlockBuddy.Interfaces.Behaviors;
using FlockBuddy.SteeringBehaviors;
using Microsoft.Xna.Framework;
using PrimitiveBuddy;

namespace FlockBuddy.Drawers
{
    public class BoidDrawer : MoverDrawer
    {
        public BoidDrawer()
        {
        }

        /// <summary>
        /// Draw the detection circle and point out all the neighbors
        /// </summary>
        /// <param name="curTime"></param>
        public override void DrawNeigborQuery(IMover mover, IPrimitive prim, Color color)
        {
            var boid = mover as IBoid;

            ////draw the query cells
            //MyFlock.CellSpace.RenderCellIntersections(prim, Position, QueryRadius, Color.Green);

            ////get the query rectangle
            //var queryRect = CellSpacePartition<Boid>.CreateQueryBox(Position, QueryRadius);
            //prim.Rectangle(queryRect, Color.White);

            //get the query circle
            prim.Circle(boid.Position, boid.NeighborsQueryRadius, color);

            ////draw the neighbor dudes
            //List<IMover> neighbors = MyFlock.FindNeighbors(this, QueryRadius);
            //foreach (var neighbor in neighbors)
            //{
            //	prim.Circle(neighbor.Position, neighbor.Radius, Color.Red);
            //}
        }

        public override void DrawPursuitQuery(IMover mover, IPrimitive prim)
        {
            var boid = mover as IBoid;

            boid.Behaviors.TryGetValue(BehaviorType.Pursuit, out var pursuitBehavior);
            var pursuit = pursuitBehavior as Pursuit;

            if (null != pursuit && pursuit.Prey != null)
            {
                prim.Circle(boid.Position, boid.PreyQueryRadius, Color.Red);
            }
            else
            {
                prim.Circle(boid.Position, boid.PreyQueryRadius, Color.White);
            }
        }

        public override void DrawTotalForce(IMover mover, IPrimitive prim, Color color)
        {
            var boid = mover as IBoid;

            //draw the force being applied
            prim.Line(boid.Position, boid.Position + boid.TotalForce, color);
        }

        public override void DrawWallFeelers(IMover mover, IPrimitive prim, Color color)
        {
            var boid = mover as IBoid;

            //get the wall avoidance steering behavior
            boid.Behaviors.TryGetValue(BehaviorType.WallAvoidance, out var behav);

            //draw all the whiskers
            var wallAvoidance = behav as IWallBehavior;
            if (null != wallAvoidance)
            {
                foreach (var whisker in wallAvoidance.Feelers)
                {
                    prim.Line(boid.Position, whisker, color);
                }
            }
        }

        public override void DrawSpeedForce(IMover mover, IPrimitive prim, Color color)
        {
            var boid = mover as IBoid;

            //draw a circle at the MaxForce line
            //prim.Circle(Position, MaxForce, color);

            //draw the speed force being applied
            prim.Line(boid.Position, boid.Position + boid.SpeedForce, color);
        }
    }
}