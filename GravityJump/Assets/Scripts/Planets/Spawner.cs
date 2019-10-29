using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class Spawner : ObjectManagement.Spawner
    {
        private GameObject ActivePlanet;
        private float total_frequency;

        public SpawningPoint PlayerSpawningPlanet;

        public override void InitiateSpawner()
        {
            this.spawnerType = ObjectManagement.SpawnerType.Planet;

            this.AvailablePrefabs = new List<GameObject>();
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/Planet") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/Cucumboid") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/DarkDwarf") as GameObject);

            // Computing the sum of available planets' frequency
            foreach (GameObject planet in this.AvailablePrefabs)
            {
                total_frequency += planet.GetComponent<Physic.AttractiveBody>().frequency;
            }
            // Create a starting Planet
            assetId = 0;
            position = new Vector3(-3, 0, 0);
            rotation = 0;
            scaleRatio = 1;
            GameObject generatedObject = Instantiate(
                AvailablePrefabs[assetId],
                position,
                Quaternion.Euler(0, 0, rotation));
            generatedObject.transform.localScale *= scaleRatio;
            PlayerSpawningPlanet = new SpawningPoint(generatedObject, 0, 0);
        }
        public override void PrepareNextAsset()
        {
            // Saving previous parameters
            float r_last = scaleRatio;
            float x_last = position.x;
            float y_last = position.y;
            GameObject ActivePlanet = AvailablePrefabs[assetId];
            // Deciding next planet and it size
            SetRandomAssetId();
            GameObject NextPlanet = AvailablePrefabs[assetId];
            float r = NextPlanet.GetComponent<Physic.AttractiveBody>().GetRandomSize();
            // Deducing minimal and maximal distance to the next planet
            float d_min = Mathf.Max(NextPlanet.GetComponent<Physic.AttractiveBody>().GetMinimalDistance(r), ActivePlanet.GetComponent<Physic.AttractiveBody>().GetMinimalDistance(r_last));
            float d_max = Mathf.Min(NextPlanet.GetComponent<Physic.AttractiveBody>().GetMaximalDistance(r), ActivePlanet.GetComponent<Physic.AttractiveBody>().GetMaximalDistance(r_last));
            // Deciding next planet distance to previous one
            float d = d_min + (d_max - d_min) * Random.value;
            // Defining constraints for next planet's position
            float y_limit = 5;
            float angle_margin = Mathf.PI / 8;
            float angle_min = Mathf.PI / 8;
            float angle_max = 7 * Mathf.PI / 8;
            // Planet can sometime appear slightly out of camera's range
            if (Mathf.Abs(y_last) < 5)
            {
                y_limit += r / 3;

            }
            // Finding an angle satisfying all constraints
            float teta = angle_min;
            int attempts = 0;
            float y = y_last;
            float x = x_last;
            while (attempts < 20 && (teta <= angle_min || teta >= angle_max || Mathf.Abs(y) > y_limit || Mathf.Abs(teta - Mathf.PI / 2) < angle_margin))
            {
                attempts += 1;
                teta = angle_min + (angle_max - angle_min) * Random.value;
                // Computing the y position of next planet
                y = y_last + Mathf.Cos(teta) * (r_last + d + r);
            }
            // If no compromise could be made, bringing next planet closer to center of camera
            if (attempts == 20)
            {
                Debug.Log("No random position could be selected");
                if (y_last < 0)
                {
                    teta = Mathf.PI / 4;
                }
                else
                {
                    teta = 3 * Mathf.PI / 4;
                }
                // Computing the y position of next planet
                y = y_last + Mathf.Cos(teta) * (r_last + d + r);
            }
            // Computing the x position of next planet
            x = x_last + Mathf.Sin(teta) * (r_last + d + r);
            scaleRatio = r;
            position = new Vector3(x, y, 0);
            rotation = Random.value * 360;
        }
        private void SetRandomAssetId()
        {
            // We select a random number between 0 and the sum of available planets frequencies
            // This number will define the selected planet
            float v = Random.value * total_frequency;
            float f;
            assetId = 0;
            foreach (GameObject planet in this.AvailablePrefabs)
            {
                f = planet.GetComponent<Physic.AttractiveBody>().frequency;
                if (v <= f)
                {
                    return;
                }
                else
                {
                    v -= f;
                    assetId += 1;
                }
            }
            // In case nothing got selected
            // Should not happen as long as AvailablePrefabs and their frequencies remain unchanged
            Debug.Log("No random planet could be selected");
            assetId = 0;
        }
    }
}
