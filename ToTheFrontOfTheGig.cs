

using NUnit.Framework;
using NUnitLite;
using System;
using System.Reflection;
using System.Collections.Generic;

// To execute C#, please define "static void Main" on a class
// named Solution.

class Solution
{
    public static int Main(string[] args)
    {
        return new AutoRun(Assembly.GetCallingAssembly())
            .Execute(new String[] { "--labels=All" });
    }

    public class Friend : IComparable<Friend>
    {
        public int Key;
        public long Position;

        public Friend(int key, long position)
        {
            this.Key = key;
            this.Position = position;
        }
        public void MoveTo(long newPositon)
        {
            this.Position = newPositon;
        }

        public int CompareTo(Friend compareFriend)
        {
            if (compareFriend == null)
                return 1;
            else
                return this.Position.CompareTo(compareFriend.Position);
        }
    }


    public class Calculator
    {
        public LinkedList<Friend> FriendsInOrder = new LinkedList<Friend>();
        public Dictionary<long, bool> RowsInUse = new Dictionary<long, bool>();


        public Calculator(long[] P)
        {
            this.FriendsInOrder = this.BuildFriendsByOrder(P);
            this.RowsInUse = this.BuildRowsInUse(P);
        }

        public Dictionary<long, bool> BuildRowsInUse(long[] pos)
        {
            var rowInUse = new Dictionary<long, bool>();
            for (var i = 0; i < pos.Length; i++)
            {
                var pad = pos[i];
                if (rowInUse.ContainsKey(pad))
                {
                    rowInUse[pad] = true;
                }
                else
                {
                    rowInUse.Add(pad, true);
                }
            }

            return rowInUse;
        }

        public LinkedList<Friend> BuildFriendsByOrder(long[] pos)
        {
            var friends = new List<Friend>();

            for (var i = 0; i < pos.Length; i++)
            {
                var frog = new Friend(i, pos[i]);
                friends.Add(frog);
            }

            friends.Sort();

            var orderedFriends = new LinkedList<Friend>(friends);

            return orderedFriends;
        }

        public Friend LastPlaceFriendStillStanding()
        {
            return FriendsInOrder.First.Value;
        }

        public bool FriendsStllStanding()
        {
            if (FriendsInOrder == null)
            {
                return false;
            }

            return (FriendsInOrder.Count > 0);
        }

        public bool IsFriendOnRow(long pad)
        {
            return RowsInUse.ContainsKey(pad);
        }

        public long NextFreeRow(Friend friend, long frontRow)
        {
            for (var nextRow = friend.Position + 1; nextRow < frontRow; nextRow++)
            {
                if (this.IsFriendOnRow(nextRow) == false)
                {
                    return nextRow;
                }
            }
            return frontRow;
        }

        public long GetSecondsRequired(long N, int F)
        {

            var frontRow = N;
            var friends = F;

            var steps = 0;

            while (this.FriendsStllStanding())
            {
                steps++;

                var targetFriendsToMove = this.LastPlaceFriendStillStanding();

                //Always free up the row and the friend
                this.RowsInUse.Remove(targetFriendsToMove.Position);
                this.FriendsInOrder.RemoveFirst();  //Because its sorted the last place will always be first

                var moveTo = this.NextFreeRow(targetFriendsToMove, N);

                if (moveTo < frontRow)
                {
                    //Mark that this is now in use
                    this.RowsInUse[moveTo] = true;

                    //Now move the friend
                    targetFriendsToMove.Position = moveTo;

                    //Is it always going to the first or last?
                    if (this.FriendsInOrder.Count > 0 && targetFriendsToMove.Position > this.FriendsInOrder.First.Value.Position)
                    {
                        //They are now at the front
                        this.FriendsInOrder.AddLast(targetFriendsToMove);
                    }
                    else
                    {
                        //They have moved forward but are still last
                        this.FriendsInOrder.AddFirst(targetFriendsToMove);
                    }


                }
            }

            return steps;
        }


    }

    [TestFixture]
    public class FriendTests
    {
        public Calculator GetTarget(long[] P)
        {
            return new Calculator(P);
        }



        [Test]
        public void Example_1()
        {


            var target = GetTarget(new long[] { 1 });

            var seconds = target.GetSecondsRequired(3, 1);

            Assert.AreEqual(2, seconds);
        }


        [Test]
        public void Example_2()
        {

            var target = GetTarget(new long[] { 5, 2, 4 });

            var seconds = target.GetSecondsRequired(6, 3);

            Assert.AreEqual(4, seconds);
        }


    }
}
