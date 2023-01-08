

```cs
// editor__window_log

LOG.str(sum);
```

<br/><br/>

```cs

float t_prev = this.t;

while (true)
{
	//
	Tr_pos.position = bezier_path.pos(this.t);


	string sum = "";
	for (int i = 0; i < (int)(t * 10); i += 1) sum += i.ToString() + ' ';
	sum += '\n';
	for (int i = 0; i < (int)(t * 10) - 1; i += 1) sum += "__";
	sum += "^";

	if(Mathf.Abs(t - t_prev) > 0.01f)
	{
		LOG.str(sum);
		t_prev = t;
	}

	yield return null;
}

```


![Capture](https://user-images.githubusercontent.com/83577810/211186319-6247e08d-3060-48a7-bb1b-6bf0144b44fc.PNG)

