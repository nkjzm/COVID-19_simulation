using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private Human humanPrefab = null;
		[SerializeField] Vector2 generationSize = Vector2.one;
		[SerializeField] private int generationCount = 50;

		private List<Human> humans = new List<Human>();

		private void Start()
		{
			for (int i = 0; i < generationCount; i++)
			{
				var pos = new Vector2(
					Random.Range(-generationSize.x, generationSize.x),
					Random.Range(-generationSize.y, generationSize.y)
				);
				var human = Instantiate(humanPrefab, pos, Quaternion.identity, transform);
				humans.Add(human);
				if (i == 0)
				{
					human.CurrentState = Human.State.Sick;
				}
			}

			StartCoroutine(UpdateGage());
		}

		[SerializeField] private Transform gageParent = null;

		[SerializeField] float gageXMin = -10;
		[SerializeField] float gageYMin = -10;
		[SerializeField] private float gageWidth = 0.2f;
		[SerializeField] private LineRenderer lineRendererPrefab = null;

		IEnumerator UpdateGage()
		{
			var scale = 0.1f;
			while (true)
			{
				float min = gageYMin;
				
				var sicCount = humans.Where(h => h.CurrentState == Human.State.Sick).Count();
				CreateLine(gageXMin, min * scale, (min + sicCount) * scale, new Color(1f, 0.5f, 0.5f));
				min += sicCount;

				var healthCount = humans.Where(h => h.CurrentState == Human.State.Healthy).Count();
				CreateLine(gageXMin, min * scale, (min+healthCount) * scale, Color.white);
				min += healthCount;


				var recoveredCount = humans.Where(h => h.CurrentState == Human.State.Recovered).Count();
				CreateLine(gageXMin, min * scale, (min + recoveredCount) * scale, new Color(0.5f, 1f, 0.5f));
				min += recoveredCount;

				gageXMin += gageWidth;
				yield return new WaitForSeconds(0.2f);
			}
		}

		void CreateLine(float x, float minY, float maxY, Color color)
		{
			var line = Instantiate(lineRendererPrefab, gageParent);
			line.transform.SetParent(gageParent);
			var positions = new Vector3[2]
			{
				new Vector3(x, minY, 0),
				new Vector3(x, maxY, 0)
			};
			line.SetPositions(positions);
			line.startColor = line.endColor = color;
			line.startWidth = line.endWidth = gageWidth;
		}

		[SerializeField] private UnityEngine.UI.Text health, sick, recovered;

		private void Update()
		{
			health.text = humans.Where(h => h.CurrentState == Human.State.Healthy).Count().ToString();
			sick.text = humans.Where(h => h.CurrentState == Human.State.Sick).Count().ToString();
			recovered.text = humans.Where(h => h.CurrentState == Human.State.Recovered).Count().ToString();
		}
	}
}