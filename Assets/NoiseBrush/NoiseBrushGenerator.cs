using UnityEngine;
using System.Collections;

	[System.Serializable]
	public class NoiseBrushNoise
	{
		public int seed = 12345;
		public float amount = 5;
		public float size = 0.3f;
		public float detail = 1;
		public float uplift = 0.4f;

		public void Generate (float[] heights, int sizeX, int sizeZ, int shiftX, int shiftZ) //note that shiftX and Z swapped
		{
			Random.seed = seed;
			float randomX = Random.Range(0,10000);
			float randomZ = Random.Range(0,10000);

			float curSize = Mathf.Min(sizeX,sizeZ)*size + 0.01f;
			float curAmount = amount;
			
			for (int i=0; i<100;i++)
			{
				for (int x=0; x<sizeX; x++)
					for (int z=sizeZ-1; z>=0; z--)
				{
					heights[z*sizeX + x] -= Mathf.PerlinNoise(
						(x+shiftX)/curSize + randomX, 
						(z+shiftZ)/curSize + randomZ
							) * curAmount - curAmount*uplift;
				}

				curSize = (curSize/2)-1;
				curAmount = (curAmount/2+detail)-1;

				if (curSize<1 || curAmount<0) break;
			}
		}
	}

