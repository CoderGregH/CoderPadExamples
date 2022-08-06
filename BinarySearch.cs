using System;

using NUnit.Framework;
using NUnitLite;
using System;
using System.Reflection;


public class Runner
{
    public static int Main(string[] args)
    {
        return new AutoRun(Assembly.GetCallingAssembly())
            .Execute(new String[] { "--labels=All" });
    }

    public class BinarySearch
    {
        int[] _nums;
        int _target;

        public BinarySearch(int[] nums, int target)
        {
            _nums = nums;
            _target = target;
        }


        public int[] searchRange()
        {
            var firstPos = searchForFirst(0, _nums.Length - 1);
            var lastPos = firstPos;

            for (var pos = firstPos + 1; pos < _nums.Length; pos++)
            {
                var valueAtPos = _nums[pos];
                if (valueAtPos == _target)
                {
                    lastPos = pos;
                }
                else
                {
                    break;
                }
            }

            return new int[] { firstPos, lastPos };
        }

        private bool IsFirst(int pos)
        {
            if (pos < 0)
            {
                return true;
            }

            return !(_nums[pos] == _target);
        }

        public int searchForFirst(int start, int end)
        {
            while (start <= end)
            {
                var mid = (start + (end - start) / 2);

                var midValue = _nums[mid];

                if (midValue == _target)
                {
                    //If its not unique make sure its the first one  
                    var isFirst = IsFirst(mid - 1);

                    if (isFirst)
                    {
                        //Great the Mid point is the target!
                        return mid;
                    }
                    //Move left as this is not the first item
                    start = start - 1;
                }
                else
                {
                    if (_target < midValue)
                    {
                        //Move the end to mid -1
                        end = mid - 1; // the 'target' can be in the first half
                    }
                    else
                    {
                        //Move the start to mid +1  
                        start = mid + 1; // the 'target' can be in the second half
                    }
                }
            }

            return -1;
        }

    }

    [TestFixture]
    public class BinarySearchTests
    {
        private BinarySearch GetTarget(int[] nums, int targetToFind)
        {
            return new BinarySearch(nums, targetToFind);
        }


        [Test]
        public void searchForFirst_Should_Find_First_Pos()
        {
            var nums = new int[] { 1, 5, 7, 7, 7, 7, 10 };

            var testMe = GetTarget(nums, 7);

            var result = testMe.searchForFirst(0, nums.Length - 1);

            Assert.AreEqual(result, 2);
        }

        [Test]
        public void searchRange_Should_Return_Start_And_End()
        {
            var nums = new int[] { 1, 5, 7, 7, 7, 7, 10 };

            var testMe = GetTarget(nums, 7);

            var result = testMe.searchRange();


            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], 2);
            Assert.AreEqual(result[1], 5);

        }

        [Test]
        public void searchRange_Should_Return_Start_And_End_Where_Not_Mid()
        {
            var nums = new int[] { 5, 7, 7, 8, 8, 10 };

            var testMe = GetTarget(nums, 8);

            var result = testMe.searchRange();


            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], 3);
            Assert.AreEqual(result[1], 4);

        }

        [Test]
        public void searchRange_Should_Return_Start_And_End_Where_The_Same()
        {
            var nums = new int[] { 5, 7, 7, 8, 8, 10, 11 };

            var testMe = GetTarget(nums, 10);

            var result = testMe.searchRange();


            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], 5);
            Assert.AreEqual(result[1], 5);

        }

        [Test]
        public void searchRange_Should_Return_Even_When_All_The_Same()
        {
            var nums = new int[] { 5, 5, 5, 5, 5, 5 };

            var testMe = GetTarget(nums, 5);

            var result = testMe.searchRange();


            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0], 0);
            Assert.AreEqual(result[1], 5);

        }

    }
}
