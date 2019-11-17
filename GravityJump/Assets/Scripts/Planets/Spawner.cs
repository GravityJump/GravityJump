using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class Spawner : ObjectManagement.Spawner
    {
        public SpawningPoint PlayerSpawningPlanet;

        public override void InitiateSpawner()
        {
            this.SpawnerType = ObjectManagement.SpawnerType.Planet;

            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/Planet") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/HotPlanet") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/Cucumboid") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/DarkDwarf") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Planetoids/Treats/Candy") as GameObject);

            // Create a starting Planet
            this.AssetId = 0;
            this.Position = new Vector3(-3, 0, 0);
            this.Rotation = 0;
            this.ScaleRatio = 1;
            GameObject generatedObject = Instantiate(
                AvailablePrefabs[this.AssetId],
                this.Position,
                Quaternion.Euler(0, 0, this.Rotation));
            generatedObject.transform.localScale *= this.ScaleRatio;
            this.PlayerSpawningPlanet = new SpawningPoint(generatedObject, 0, 0);
        }
        public override void PrepareNextAsset()
        {
            // Saving previous parameters
            float r_last = this.ScaleRatio;
            float x_last = this.Position.x;
            float y_last = this.Position.y;
            GameObject activePlanet = this.AvailablePrefabs[this.AssetId];
            // Deciding next planet and it size
            this.SetRandomAssetId();
            GameObject nextPlanet = this.AvailablePrefabs[this.AssetId];
            float r = nextPlanet.GetComponent<Physic.AttractiveBody>().GetRandomSize();
            // Deducing minimal and maximal distance to the next planet
            float d_min = Mathf.Max(nextPlanet.GetComponent<Physic.AttractiveBody>().GetMinimalDistance(r), activePlanet.GetComponent<Physic.AttractiveBody>().GetMinimalDistance(r_last));
            float d_max = Mathf.Min(nextPlanet.GetComponent<Physic.AttractiveBody>().GetMaximalDistance(r), activePlanet.GetComponent<Physic.AttractiveBody>().GetMaximalDistance(r_last));
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
            this.ScaleRatio = r;
            this.Position = new Vector3(x, y, 0);
            this.Rotation = Random.value * 360;
        }
        public override float GetFrequency(int assetId)
        {
            return this.AvailablePrefabs[assetId].GetComponent<Physic.AttractiveBody>().Frequency;
        }
    }
}
