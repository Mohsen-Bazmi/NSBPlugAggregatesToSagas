using NServiceBus;

namespace EndPoint.Sagas
{
    public class UserSaga : Saga
    {
        protected override void ConfigureHowToFindSaga(IConfigureHowToFindSagaWithMessage sagaMessageFindingConfiguration)
        {
            throw new System.NotImplementedException();
        }
    }
}