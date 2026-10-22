import asyncio
from app.learning import LearningStore
from app.api_facade import BrokerFacade


def test_learning_persists(tmp_path):
    p=tmp_path/'learning.json'; store=LearningStore(p)
    store.record(action='HOLD',instrument='uid',outcome='unknown')
    summary=LearningStore(p).summary()
    assert summary['observations']==1 and summary['actions']['HOLD']==1


def test_facade_retries():
    class Service:
        def __init__(self): self.n=0
        async def get_info(self):
            self.n+=1
            if self.n<2: raise RuntimeError()
            return {'ok':True}
    class Services: users=Service()
    class Broker: services=Services()
    result=asyncio.run(BrokerFacade(Broker()).user_info())
    assert result['ok'] is True and Broker.services.users.n==2
