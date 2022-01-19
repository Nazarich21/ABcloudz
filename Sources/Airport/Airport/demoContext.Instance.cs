namespace Airport
{
    public partial class demoContext
    {
        private static demoContext _instance = null;
        public static demoContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new demoContext();
                }
                return _instance;
            }
        }
    }
}