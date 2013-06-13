using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace BellRingingKata
{
	public class Configuration
	{
	    private bool everyoneChanges;
		public Configuration ()
		{
		    everyoneChanges = true;
			Positions = new[]{1,2,3,4,5,6,7,8};
		}

		public int[] Positions {get; private set;}

		public void ChangePositions()
		{
		    if (everyoneChanges)
		        AllChange();
		    else
		        AllButEndsChange();
		    everyoneChanges = !everyoneChanges;
		}

	    private void AllButEndsChange()
	    {
            var list = new int[8];
            for (int bellRinger = 1; bellRinger < Positions.Count()-1; bellRinger += 2)
            {
                SwapWithNeighbour(list, bellRinger);
            }
	        list[0] = Positions[0];
	        list[Positions.Last()] = Positions.Last();
            Positions = list;
	    }

	    private void SwapWithNeighbour(int[] list, int bellRinger)
	    {
	        list[bellRinger] = Positions[bellRinger + 1];
	        list[bellRinger + 1] = Positions[bellRinger];
	    }

	    private void AllChange()
	    {
	        var list = new int[8];
	        for (int bellRinger = 0; bellRinger < Positions.Count(); bellRinger += 2)
	        {
	            SwapWithNeighbour(list, bellRinger);
	        }
	        Positions = list;
	    }
	}

	public class BellSequence
	{
		Configuration currentConfiguration;

		public BellSequence ()
		{
			currentConfiguration = new Configuration();
		}

		public IEnumerable<int> GetBells(int requestedBell)
		{
			var index =0;
			while(index < requestedBell){
				if(index % 8 == 0 && index != 0){
					currentConfiguration.ChangePositions();
				}
				yield return currentConfiguration.Positions[index%8];
				index++;
			}
		}
	}

	[TestFixture]
	public class Tests {
		[TestCase(1,1)]
		[TestCase(2,2)]
		[TestCase(3,3)]
		[TestCase(4,4)]
		[TestCase(5,5)]
		[TestCase(6,6)]
		[TestCase(7,7)]
		[TestCase(8,8)]
		[TestCase(9,2)]
		[TestCase(10,1)]
		[TestCase(11,4)]
		[TestCase(18,4)]
		[TestCase(26,2)]
        [TestCase(32, 5)]
		public void CorrectPositionForBell(int bell, int expectPosition){
			Assert.That(new BellSequence().GetBells(bell).Last(), Is.EqualTo(expectPosition));
		}
	}

	[TestFixture]
	public class ConfigurationTests{
		[Test]
		public void ChangingPositionsOnce(){
			var subject = new Configuration();
			subject.ChangePositions();

			CollectionAssert.AreEqual(subject.Positions,new List<int>{2, 1, 4, 3, 6, 5, 8, 7});
		}

		[Test]
		public void ChangingPositionsTwice(){
			var subject = new Configuration();
			subject.ChangePositions();
			subject.ChangePositions();

			CollectionAssert.AreEqual(subject.Positions, new List<int>{2, 4, 1, 6, 3, 8, 5, 7});
		}
	}
}
