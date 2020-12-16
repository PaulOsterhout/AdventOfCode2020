using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020
{
    public class UnitTest1
    {
        private int GetExpenseAmount(int[] values)
        {
            return GetTotal(values) == 2020 ? GetMultipleOf(values) : -1;
        }

        private int GetMultipleOf(int[] values)
        {
            int multiple = 1;
            foreach (int value in values)
            {
                multiple *= value;
            }
            return multiple;
        }

        private int GetTotal(int[] values)
        {
            return values.Sum();
        }

        private List<int> RetreiveInternalList()
        {
            return StringListRetriever.Retreive("InputList1.txt").Select(x => int.Parse(x)).ToList();
        }

        [Fact]
        public void FindExpenseAmount1()
        {
            List<int> entries = RetreiveInternalList();
            int count = entries.Count;
            Assert.Equal(200, count);
            Assert.Equal(1801, entries[0]);
            Assert.Equal(1662, entries[199]);
            int expenseAmount = 0;
            for (int index1 = 0; index1 < 199; index1++)
            {
                for (int index2 = index1 + 1; index2 < 200; index2++)
                {
                    int[] values = { entries[index1], entries[index2] };
                    expenseAmount = GetExpenseAmount(values);
                    if (expenseAmount > -1) break;
                }
                if (expenseAmount > -1) break;
            }
            Assert.Equal(468051, expenseAmount);
        }

        [Fact]
        public void FindExpenseAmount2()
        {
            List<int> entries = RetreiveInternalList();
            int count = entries.Count;
            int expenseAmount = 0;
            for (int index1 = 0; index1 < (count - 2); index1++)
            {
                for (int index2 = index1 + 1; index2 < (count - 1); index2++)
                {
                    for (int index3 = index2 + 1; index3 < count; index3++)
                    {
                        int[] values = { entries[index1], entries[index2], entries[index3] };
                        expenseAmount = GetExpenseAmount(values);
                        if (expenseAmount > -1) break;
                    }
                    if (expenseAmount > -1) break;
                }
                if (expenseAmount > -1) break;
            }
            Assert.Equal(272611658, expenseAmount);
        }
    }
}
