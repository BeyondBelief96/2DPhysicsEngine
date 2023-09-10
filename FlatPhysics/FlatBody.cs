namespace FlatPhysics
{
    public enum ShapeType
    {
        Circle = 0,
        Box = 1
    }

    public sealed class FlatBody
    {
        #region Fields

        private FlatVector _linearVelocity;
        private float _rotation;
        private float _rotationalVelocity;

        public readonly float Density;
        public readonly float Mass;
        public readonly float Restitution;
        public readonly float Area;
        public readonly bool IsStatic;

        public readonly float Radius;
        public readonly float Width;
        public readonly float Height;

        public ShapeType ShapeType;

        #endregion

        #region Properties

        public FlatVector Position { get; private set; }

        #endregion

        #region Constructors

        private FlatBody(FlatVector position, float density, float mass, float restitution, float area,
            bool isStatic, float radius, float width, float height, ShapeType shapeType)
        {
            Position = position;
            _linearVelocity = FlatVector.Zero;
            _rotation = 0f;
            _rotationalVelocity = 0f;

            Density = density;
            Mass = mass;
            Restitution = restitution;
            Area = area;

            IsStatic = isStatic;
            Radius = radius;
            Width = width;
            Height = height;
            ShapeType = shapeType;
        }

        #endregion

        #region Public Methods

        public void Move(FlatVector moveVector) 
        {
            Position += moveVector;
        }

        public void MoveTo(FlatVector position)
        {
            Position = position;
        }

        public static bool CreateCircleBody(float radius, FlatVector position, float density, bool isStatic,
            float restitution, out FlatBody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;

            float area = radius * radius * MathF.PI;

            if(area < FlatWorld.MinBodySize)
            {
                errorMessage = $"Circle radius is to small. Min circle area is {FlatWorld.MinBodySize}";
                return false;
            }

            if(area > FlatWorld.MaxBodySize)
            {
                errorMessage = $"Circle radius is to large. Max circle area is {FlatWorld.MaxBodySize}";
                return false;
            }

            if(density < FlatWorld.MinDensity)
            {
                errorMessage = $"Density is to small. Min density is {FlatWorld.MinDensity}";
                return false;
            }

            if (density > FlatWorld.MaxDensity)
            {
                errorMessage = $"Density is to large. Max density is {FlatWorld.MaxDensity}";
                return false;
            }

            restitution = FlatMath.Clamp(restitution, 0f, 1f);

            // mass = area * density
            float mass = area * density;

            body = new FlatBody(position, density, mass, restitution, area, isStatic, radius, 0f, 0f, ShapeType.Circle);
            return true;
        }

        public static bool CreateBoxBody(float width, float height, FlatVector position, float density, bool isStatic,
            float restitution, out FlatBody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;

            float area = width * height;

            if (area < FlatWorld.MinBodySize)
            {
                errorMessage = $"Area is to small. Min area is {FlatWorld.MinBodySize}";
                return false;
            }

            if (area > FlatWorld.MaxBodySize)
            {
                errorMessage = $"Area is to large. Max area is {FlatWorld.MaxBodySize}";
                return false;
            }

            if (density < FlatWorld.MinDensity)
            {
                errorMessage = $"Density is to small. Min density is {FlatWorld.MinDensity}";
                return false;
            }

            if (density > FlatWorld.MaxDensity)
            {
                errorMessage = $"Density is to large. Max density is {FlatWorld.MaxDensity}";
                return false;
            }

            restitution = FlatMath.Clamp(restitution, 0f, 1f);

            // mass = area * density
            float mass = area * density;

            body = new FlatBody(position, density, mass, restitution, area, isStatic, 0f, width, height, ShapeType.Box);
            return true;
        }

        #endregion
    }
}
