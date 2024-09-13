using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Prototypes.Performance
{
    public class ArrayVsList : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text arrayTime;
        [SerializeField] private TMP_Text listTime;
        [SerializeField] private TMP_Text arrayWriteTime;
        [SerializeField] private TMP_Text listWriteTime;
        [SerializeField] private TMP_Text bubbleSortTime;
        [SerializeField] private TMP_Text mergeSortTime;

        [Header("CUSTOMIZE")]
        [SerializeField] private int containerSize;

        private float[] _array;
        private float[] _unsortedArray;
        private List<float> _list;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            TestArray();
            TestList();
            TestWriteArray();
            TestWriteList();

            MeasureBubbleSortTime();
            MeasureMergeSortTime();
        }

        private void Init()
        {
            _array = new float[containerSize];
            _unsortedArray = new float[containerSize];

            _list = new List<float>();

            for (int i = 0; i < containerSize; i++)
            {
                _array[i] = i;
                _unsortedArray[i] = UnityEngine.Random.Range(0, containerSize);

                _list.Add(i);
            }
        }

        private void TestArray()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            float total = 0;

            foreach (var item in _array)
            {
                total += item;
            }

            stopwatch.Stop();

            arrayTime.text = $"{stopwatch.ElapsedTicks} ticks - {stopwatch.ElapsedMilliseconds} ms";
        }

        private void TestList()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            float total = 0;

            foreach (var item in _list)
            {
                total += item;
            }

            stopwatch.Stop();

            listTime.text = $"{stopwatch.ElapsedTicks} ticks - {stopwatch.ElapsedMilliseconds} ms";
        }

        private void TestWriteArray()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < containerSize; i++)
            {
                _array[i] = i - 1;
            }

            stopwatch.Stop();

            arrayWriteTime.text = $"{stopwatch.ElapsedTicks} ticks - {stopwatch.ElapsedMilliseconds} ms";
        }

        private void TestWriteList()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < containerSize; i++)
            {
                _list[i] = i - 1;
            }

            stopwatch.Stop();

            listWriteTime.text = $"{stopwatch.ElapsedTicks} ticks - {stopwatch.ElapsedMilliseconds} ms";
        }

        private void MeasureBubbleSortTime()
        {
            MeasureTime(
                taskToMeasure: BubbleSort.BubbleSortAlgorithmAsync(_unsortedArray),
                textDisplayResult: bubbleSortTime
            );
        }

        private void MeasureMergeSortTime()
        {
            MeasureTime(
                taskToMeasure: MergeSort.MergeSortAlgorithmAsync(_unsortedArray, 0, _unsortedArray.Length - 1),
                textDisplayResult: mergeSortTime
            );
        }

        private async void MeasureTime(Task taskToMeasure, TMP_Text textDisplayResult)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            await taskToMeasure;

            // taskToMeasure?.Invoke();

            stopwatch.Stop();

            textDisplayResult.text = $"{stopwatch.ElapsedTicks} ticks - {stopwatch.ElapsedMilliseconds} ms";
        }
    }
}
