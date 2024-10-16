using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Saferio.Util.SaferioTween;
using UnityEngine;

public class SequentialSlide : IPageViewAnimation
{
    private RectTransform[] _items;
    private int _rowNumber;
    private int _columnNumber;

    public RectTransform[] Items
    {
        set => _items = value;
    }

    public int RowNumber
    {
        set => _rowNumber = value;
    }

    public int ColumnNumber
    {
        set => _columnNumber = value;
    }

    public void PlayPageTransition()
    {
        ScaleDown();

        void ScaleDown()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                int index = i;

                SaferioTween.Scale(_items[i], Vector3.zero, 0.2f,
                    onCompletedAction: () =>
                    {
                        if (index == _items.Length - 1)
                        {
                            SlideSequential();
                        }
                    }
                );
            }
        }

        void SlideSequential()
        {
            Canvas canvas = TransformUtil.GetCanvasFromParents(_items[0]);

            Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

            for (int i = 0; i < _rowNumber; i++)
            {
                for (int j = 0; j < _columnNumber; j++)
                {
                    int index = j + i * _columnNumber;

                    SaferioTween.Delay(1 - (float)i / _rowNumber,
                        onCompletedAction: () =>
                        {
                            Vector2 initialPosition = _items[index].localPosition;

                            UIUtil.SetLocalPositionY(_items[index], initialPosition.y + canvasSize.y);

                            _items[index].localScale = Vector3.one;

                            SaferioTween.LocalPosition(_items[index], initialPosition, 0.4f);
                        }
                    );
                }
            }
        }
    }
}
