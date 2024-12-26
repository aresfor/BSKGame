using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;

namespace Game.Gameplay
{

    public static class RandomUtils
    {
        static Math.Random localRand = new Math.Random(1851936439);

        public static void worldRandomSetSeed(uint seed)
        {
            localRand.InitState(seed);
        }

        public static int worldRandomInt()
        {
            return localRand.NextInt();
        }

        public static int worldRandomInt(int min, int max)
        {
            return localRand.NextInt(min, max);
        }

        public static uint worldRandomUint(uint min, uint max)
        {
            return localRand.NextUInt(min, max);
        }

        public static float worldRandomFloat(float min, float max)
        {
            return localRand.NextFloat(min, max);
        }

        //随机min,max之间的不重复乱序数组(num为校验值，如果取num个不重复的值，可以直接取返回数组的前num个元素即可)
        public static int[] randomNoRepetitonNumber(int min, int max, int num)
        {
            if (max - min < num)
            {
                return null;
            }

            int[] nums = Enumerable.Range(min, max).OrderBy(x => worldRandomInt()).ToArray();
            return nums;
        }

        public static T RandomSelectByWeight<T>(List<T> datas, List<uint> weights)
        {
            if (datas.Count == 0 || weights.Count == 0 || datas.Count != weights.Count)
            {
                Logs.Error($"RandomSelectByWeight datas.Count{datas.Count} or weights.Count:{weights.Count} error!");
                return default(T);
            }

            int index = 0;
            int rate = 0;
            System.Random rnd = new System.Random((int)DateTime.Now.Ticks);
            int randomRate = rnd.Next();
            int originRandomRate = randomRate;
            int i = 0;
            while (i < weights.Count)
            {
                rate += (int)weights[i];
                if (randomRate < rate)
                {
                    index = i;
                    Logs.Debug(
                        $" RandomSelectByWeight, originRandomRate:{originRandomRate}, randomRate:{randomRate}, selectId:{datas[index]}");
                    break;
                }
                else if (i == weights.Count - 1)
                {
                    randomRate = randomRate % rate;
                    i = 0;
                    rate = 0;
                    continue;
                }

                i++;
            }

            return datas[index];
        }
    }
}