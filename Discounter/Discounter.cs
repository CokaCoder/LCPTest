namespace Test.Discounter
{
    public abstract class Discounter

    {
        #region Properties
        protected Projector Projector;
        #endregion

        #region Constructor
        protected Discounter(Projector projector)
        {
             Projector = projector;
        }
        #endregion
    }
}