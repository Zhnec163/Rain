using UnityEngine;
using Random = UnityEngine.Random;

public class RandomHelper : MonoBehaviour
{
    public static Color GetRandomColor()
    {
        float minRangeColor = 0F;
        float maxRangeColor = 255F;
        float r = Random.Range(minRangeColor, maxRangeColor) / maxRangeColor;
        float g = Random.Range(minRangeColor, maxRangeColor) / maxRangeColor;
        float b = Random.Range(minRangeColor, maxRangeColor) / maxRangeColor;
        int alpha = 1;
        return new Color(r, g, b, alpha);
    }
    
    public static Vector3 GetRandomPositionOver(Transform positionOver, Transform positionOrigin)
    {
        float coordinateX = positionOver.position.x;
        float coordinateZ = positionOver.position.z;
        float scaleX = positionOver.localScale.x;
        float scaleZ = positionOver.localScale.z;
        float minCoordinateX = coordinateX - SplitInHalf(scaleX);
        float maxCoordinateX = coordinateX + SplitInHalf(scaleX);
        float minCoordinateZ = coordinateZ - SplitInHalf(scaleZ);
        float maxCoordinateZ = coordinateZ + SplitInHalf(scaleZ);
        return new Vector3(RandomHelper.GetRandomNumber(minCoordinateX, maxCoordinateX), positionOrigin.position.y, RandomHelper.GetRandomNumber(minCoordinateZ, maxCoordinateZ));
    }
    
    public static float GetRandomNumber(float min, float max)
    {
        return Random.Range(min, max);
    }
    
    private static float SplitInHalf(float coordinate)
    {
        float divider = 2;
        return coordinate / divider;
    }
}