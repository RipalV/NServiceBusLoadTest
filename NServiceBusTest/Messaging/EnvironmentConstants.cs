namespace NServiceBusTest.Messaging
{
    public static class EnvironmentConstants
    {
        public static int BatchSize
        {
            get
            {
                return 3;
            }
        }

        public static int MaxDeliveryCount 
        {
            get
            {
                return 3;
            }
        }

        public static int LockDuration 
        {
            get
            {
                return 120000; // 2 minutes
            }
        }
    }
}