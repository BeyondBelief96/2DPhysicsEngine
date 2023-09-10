using Flat;
using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FlatMath = FlatPhysics.FlatMath;

namespace FlatPhysics2DSandbox
{
    public class GameWorld : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private Screen _screen;
        private Sprites _sprites;
        private Shapes _shapes;
        private Camera _camera;

        private List<FlatBody> _bodies;
        private Color[] _colors;

        #endregion

        #region Ctor

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SynchronizeWithVerticalRetrace = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;

            const double UpdatesPerSecond = 60d;
            TargetElapsedTime = TimeSpan.FromTicks((long)Math.Round((double)TimeSpan.TicksPerSecond / UpdatesPerSecond));
        }

        #endregion

        #region Methods

        protected override void Initialize()
        {
            FlatUtil.SetRelativeBackBufferSize(_graphics, 0.85f);

            _screen = new Screen(this, 1280, 768);
            _sprites = new Sprites(this);
            _shapes = new Shapes(this);
            _camera = new Camera(_screen);
            _camera.Zoom = 24;

            _camera.GetExtents(out float left, out float right, out float bottom, out float top);

            int bodyCount = 10;
            float padding = Math.Abs((right - left)) * 0.05f;
            _bodies = new List<FlatBody>(bodyCount);
            _colors = new Color[bodyCount];
            

            for(int i = 0; i < bodyCount; i++)
            {
                int type = (int)ShapeType.Circle;
                FlatBody body = null;
                float x = RandomHelper.RandomSingle(left + padding, right - padding);
                float y = RandomHelper.RandomSingle(bottom + padding, top - padding);


                if(type == (int)ShapeType.Circle)
                {
                    if(!FlatBody.CreateCircleBody(1f, new FlatVector(x, y), 2f, false, 0.5f, out body, out string errorMessage))
                    {
                        throw new Exception(errorMessage);
                    }
                }
                else if(type == (int)ShapeType.Box)
                {
                    if (!FlatBody.CreateBoxBody(1f, 1f, new FlatVector(x, y), 2f, false, 0.5f, out body, out string errorMessage))
                    {
                        throw new Exception(errorMessage);
                    }
                }
                else
                {
                    throw new Exception("Unknown type");
                }

                _bodies.Add(body);
                _colors[i] = RandomHelper.RandomColor();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            FlatKeyboard keyboard = FlatKeyboard.Instance;
            FlatMouse mouse = FlatMouse.Instance;

            keyboard.Update();
            mouse.Update();
            if(keyboard.IsKeyAvailable)
            {
                if(keyboard.IsKeyClicked(Keys.Escape))
                { 
                    this.Exit();
                }

                if (keyboard.IsKeyClicked(Keys.A))
                {
                    _camera.IncZoom();
                }

                if (keyboard.IsKeyClicked(Keys.Z))
                {
                    _camera.DecZoom();
                }

                float dx = 0f;
                float dy = 0f;
                float speed = 8f; // m/s
                if(keyboard.IsKeyDown(Keys.Left)) dx--;
                if (keyboard.IsKeyDown(Keys.Right)) dx++;
                if (keyboard.IsKeyDown(Keys.Up)) dy++;
                if (keyboard.IsKeyDown(Keys.Down)) dy--;

                if(dx != 0f || dy != 0f)
                {
                    FlatVector direction = new FlatVector(dx, dy);
                    FlatMath.Normalize(direction);

                    // unit vector * speed * seconds ---> 
                    FlatVector velocity = direction * speed * FlatUtil.GetElapsedTimeInSeconds(gameTime);
                    _bodies[0].Move(velocity);
                    
                }
            }

            for(int i = 0; i < _bodies.Count - 1; i++)
            {
                for(int j = i + 1; j < _bodies.Count; j++)
                {
                    FlatBody bodyA = _bodies[i];
                    FlatBody bodyB = _bodies[j];
                    if (Collisions.IntersectCircles(bodyA.Position, bodyA.Radius, bodyB.Position, bodyB.Radius,
                        out FlatVector normal, out float depth))
                    {
                        bodyA.Move(normal * (-1f * depth) / 2f);
                        bodyB.Move(normal * depth / 2f);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _screen.Set();
            GraphicsDevice.Clear(new Color(50, 60, 70));

            _shapes.Begin(_camera);

            for(int i = 0; i < _bodies.Count; i++)
            {
                FlatBody body = _bodies[i];
                Vector2 position = FlatConverter.ToVector2(body.Position);
                if(body.ShapeType is ShapeType.Circle)
                {
                    _shapes.DrawCircleFill(position, body.Radius, 26, _colors[i]);
                    _shapes.DrawCircle(position, body.Radius, 26, Color.White);
                }
                else if(body.ShapeType is ShapeType.Box)
                {
                    _shapes.DrawBox(FlatConverter.ToVector2(body.Position), body.Width, body.Height, Color.Red);
                }
            }

            _shapes.End();

            _screen.Unset();
            _screen.Present(this._sprites);

            base.Draw(gameTime);
        }

        #endregion
    }
}
