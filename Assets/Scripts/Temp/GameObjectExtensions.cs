using UnityEngine;

public static class GameObjectExtensions
{
    public static Bounds CalculatePreciseBounds(this GameObject gameObject)
    {
        Bounds bounds = new Bounds();
        bool flag = false;
        MeshFilter[] componentsInChildren1 = gameObject.GetComponentsInChildren<MeshFilter>();
        if (componentsInChildren1.Length != 0)
        {
            bounds = GetMeshBounds(componentsInChildren1[0].gameObject, componentsInChildren1[0].sharedMesh);
            flag = true;
            for (int index = 1; index < componentsInChildren1.Length; ++index)
                bounds.Encapsulate(GetMeshBounds(componentsInChildren1[index].gameObject,
                    componentsInChildren1[index].sharedMesh));
        }

        SkinnedMeshRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (componentsInChildren2.Length != 0)
        {
            Mesh mesh = new Mesh();
            if (!flag)
            {
                componentsInChildren2[0].BakeMesh(mesh);
                bounds = GetMeshBounds(componentsInChildren2[0].gameObject, mesh);
            }

            for (int index = 1; index < componentsInChildren2.Length; ++index)
            {
                componentsInChildren2[index].BakeMesh(mesh);
                bounds = GetMeshBounds(componentsInChildren2[index].gameObject, mesh);
            }

            Object.Destroy(mesh);
        }

        return bounds;
    }

    private static Bounds GetMeshBounds(GameObject gameObject, Mesh mesh)
    {
        Bounds bounds = new Bounds();
        Vector3[] vertices = mesh.vertices;
        if (vertices.Length != 0)
        {
            bounds = new Bounds(gameObject.transform.TransformPoint(vertices[0]), Vector3.zero);
            for (int index = 1; index < vertices.Length; ++index)
                bounds.Encapsulate(gameObject.transform.TransformPoint(vertices[index]));
        }

        return bounds;
    }

    public static Bounds CalculateBounds(this GameObject gameObject, bool localSpace = false)
    {
        Vector3 position = gameObject.transform.position;
        Quaternion rotation = gameObject.transform.rotation;
        Vector3 localScale = gameObject.transform.localScale;
        if (localSpace)
        {
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
        }

        Bounds bounds1 = new Bounds();
        Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
        if (componentsInChildren.Length != 0)
        {
            Bounds bounds2 = componentsInChildren[0].bounds;
            bounds1.center = bounds2.center;
            bounds1.extents = bounds2.extents;
            for (int index = 1; index < componentsInChildren.Length; ++index)
            {
                Bounds bounds3 = componentsInChildren[index].bounds;
                bounds1.Encapsulate(bounds3);
            }
        }

        if (localSpace)
        {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            gameObject.transform.localScale = localScale;
        }

        return bounds1;
    }

    /// <summary>
    /// 获取模型的Bounds
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static Bounds CalculateModelBounds(GameObject gameObject)
    {
        Bounds bounds = gameObject.CalculateBounds(false);
        return bounds;
    }
}