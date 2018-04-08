/*
The following license applies to TagussanBSpline class or TagussanBSpline module or TagussanBSpline.cs.
Original name of TagussanBSpline is BSpline.

BSpline

MIT License

Copyright © 2014 Tagussan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
 
// taken from https://github.com/Tagussan/BSpline
// tagussan website = http://tagussan.rdy.jp/blog/archives/445

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagussanBSpline
{
    float[][] points;
    int degree;
    bool copy = true;
    int dimension;
    delegate float bf(float i);
    bf baseFunc;
    int baseFuncRangeInt;

    public TagussanBSpline(Vector3[] points, int degree)
    {
        if (copy)
        {
            //this.points = [];
            this.points = new float[points.Length][];
            for (var i = 0; i < points.Length; i++)
            {
                this.points[i] = new float[] { points[i].x, points[i].y, points[i].z };
            }
        }
        else
        {
            //this.points = points;
        }
        this.degree = degree;
        dimension = 3;// points[0].le;
        if (degree == 2)
        {
            baseFunc = basisDeg2;
            baseFuncRangeInt = 2;
        }
        else if (degree == 3)
        {
            baseFunc = basisDeg3;
            baseFuncRangeInt = 2;
        }
        else if (degree == 4)
        {
            baseFunc = basisDeg4;
            baseFuncRangeInt = 3;
        }
        else if (degree == 5)
        {
            baseFunc = basisDeg5;
            baseFuncRangeInt = 3;
        }
    }

    delegate float Seq(int obj);

    Seq seqAt(int dim)
    {
        float[][] points = this.points;
        int margin = degree + 1;
        return (n) =>
        {
            if (n < margin)
            {
                return points[0][dim];
            }
            else if (points.Length + margin <= n)
            {
                return points[points.Length - 1][dim];
            }
            else
            {
                return points[n - margin][dim];
            }
        };
    }

    float basisDeg2(float x)
    {
        if (-0.5 <= x && x < 0.5)
        {
            return 0.75f - x * x;
        }
        else if (0.5 <= x && x <= 1.5)
        {
            return 1.125f + (-1.5f + x / 2.0f) * x;
        }
        else if (-1.5 <= x && x < -0.5)
        {
            return 1.125f + (1.5f + x / 2.0f) * x;
        }
        else
        {
            return 0f;
        }
    }

    float basisDeg3(float x)
    {
        if (-1 <= x && x < 0)
        {
            return 2.0f / 3.0f + (-1.0f - x / 2.0f) * x * x;
        }
        else if (1 <= x && x <= 2)
        {
            return 4.0f / 3.0f + x * (-2.0f + (1.0f - x / 6.0f) * x);
        }
        else if (-2 <= x && x < -1)
        {
            return 4.0f / 3.0f + x * (2.0f + (1.0f + x / 6.0f) * x);
        }
        else if (0 <= x && x < 1)
        {
            return 2.0f / 3.0f + (-1.0f + x / 2.0f) * x * x;
        }
        else
        {
            return 0;
        }
    }

    float basisDeg4(float x)
    {
        if (-1.5 <= x && x < -0.5)
        {
            return 55.0f / 96.0f + x * (-(5.0f / 24.0f) + x * (-(5.0f / 4.0f) + (-(5.0f / 6.0f) - x / 6.0f) * x));
        }
        else if (0.5 <= x && x < 1.5)
        {
            return 55.0f / 96.0f + x * (5.0f / 24.0f + x * (-(5.0f / 4.0f) + (5.0f / 6.0f - x / 6.0f) * x));
        }
        else if (1.5 <= x && x <= 2.5)
        {
            return 625.0f / 384.0f + x * (-(125.0f / 48.0f) + x * (25.0f / 16.0f + (-(5.0f / 12.0f) + x / 24.0f) * x));
        }
        else if (-2.5 <= x && x <= -1.5)
        {
            return 625.0f / 384.0f + x * (125.0f / 48.0f + x * (25.0f / 16.0f + (5.0f / 12.0f + x / 24.0f) * x));
        }
        else if (-1.5 <= x && x < 1.5)
        {
            return 115.0f / 192.0f + x * x * (-(5.0f / 8.0f) + x * x / 4.0f);
        }
        else
        {
            return 0;
        }
    }

    float basisDeg5(float x)
    {
        if (-2 <= x && x < -1)
        {
            return 17.0f / 40.0f + x * (-(5.0f / 8.0f) + x * (-(7.0f / 4.0f) + x * (-(5.0f / 4.0f) + (-(3.0f / 8.0f) - x / 24.0f) * x)));
        }
        else if (0 <= x && x < 1)
        {
            return 11.0f / 20.0f + x * x * (-(1.0f / 2.0f) + (1.0f / 4.0f - x / 12.0f) * x * x);
        }
        else if (2 <= x && x <= 3)
        {
            return 81.0f / 40.0f + x * (-(27.0f / 8.0f) + x * (9.0f / 4.0f + x * (-(3.0f / 4.0f) + (1.0f / 8.0f - x / 120.0f) * x)));
        }
        else if (-3 <= x && x < -2)
        {
            return 81.0f / 40.0f + x * (27.0f / 8.0f + x * (9.0f / 4.0f + x * (3.0f / 4.0f + (1.0f / 8.0f + x / 120.0f) * x)));
        }
        else if (1 <= x && x < 2)
        {
            return 17.0f / 40.0f + x * (5.0f / 8.0f + x * (-(7.0f / 4.0f) + x * (5.0f / 4.0f + (-(3.0f / 8.0f) + x / 24.0f) * x)));
        }
        else if (-1 <= x && x < 0)
        {
            return 11.0f / 20.0f + x * x * (-(1.0f / 2.0f) + (1.0f / 4.0f + x / 12.0f) * x * x);
        }
        else
        {
            return 0;
        }
    }

    float getInterpol(Seq seq, float t)
    {
        bf f = baseFunc;
        int rangeInt = baseFuncRangeInt;
        int tInt = (int)Mathf.Floor(t);
        float result = 0;
        for (int i = tInt - rangeInt; i <= tInt + rangeInt; i++)
        {
            result += seq(i) * f(t - i);
        }
        return result;
    }

    public float[] calcAt(float t)
    {
        t = t * ((degree + 1) * 2 + points.Length);//t must be in [0,1]
        if (this.dimension == 2)
        {
            return new float[] { getInterpol(seqAt(0), t), getInterpol(seqAt(1), t) };
        }
        else if (this.dimension == 3)
        {
            return new float[] { getInterpol(seqAt(0), t), getInterpol(seqAt(1), t), getInterpol(seqAt(2), t) };
        }
        else
        {
            float[] res = new float[dimension];
            for (int i = 0; i < dimension; i++)
            {
                res[i] = getInterpol(seqAt(i), t);
            }
            return res;
        }
    }
}
