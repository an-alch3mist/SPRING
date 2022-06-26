
```cs

// SPRING //
_points[0] = _start.position;
_points[_points.Count - 1] = _end.position;



for (int iter = 0; iter < 16; iter += 1)
{
    for (int i0 = 1; i0 <= _points.Count - 2; i0 += 1)
    {
        //
        _points[i0] +=
        (
              ((_points[i0 - 1] - _points[i0]) + (_points[i0 + 1] - _points[i0])) * (Ks * _points.Count / 10f) * dt +
                 new Vector2(0f, -1f) * dt
        );
        //
    }

}


```
