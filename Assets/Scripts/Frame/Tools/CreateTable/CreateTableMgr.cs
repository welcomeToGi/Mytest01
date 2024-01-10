using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTableMgr : MonoBehaviour
{
    public void CreateTable(DataTable table, Transform parent, bool active = true)
    {
        GameObject mItemPre = Resources.Load("Prefabs/TableItem") as GameObject;
        GameObject columnPre = Resources.Load("Prefabs/Column") as GameObject;
        int rowCount = table.Rows.Count;
        for (int i = 1; i < rowCount; i++)
        {
            // ������
            Transform column = Instantiate(columnPre, parent).transform;
            float maxHeight = 0;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                // �������ڵ�Ԫ��
                TableItem item = Instantiate(mItemPre, column).GetComponent<TableItem>();
                item.InitItem(table.Rows[i][j].ToString(), float.Parse(table.Rows[0][j].ToString()), i);
                LayoutRebuilder.ForceRebuildLayoutImmediate(column.GetComponent<RectTransform>());
                float itemHeight = item.GetComponentInChildren<TextMeshProUGUI>().GetComponent<RectTransform>().rect.height;
                maxHeight = maxHeight > itemHeight ? maxHeight : itemHeight;
                column.GetChild(j).GetComponentInChildren<TextMeshProUGUI>().GetComponent<ContentSizeFitter>().enabled = false;
                Debug.Log(itemHeight);
            }
            // ����ÿ�����߶�Ϊ����
            for (int j = 0; j < column.childCount; j++)
            {
                RectTransform itemRect = column.GetChild(j).GetComponentInChildren<TextMeshProUGUI>().GetComponent<RectTransform>();
                itemRect.sizeDelta = new Vector2(itemRect.rect.width, maxHeight);
                LayoutRebuilder.ForceRebuildLayoutImmediate(column.GetComponent<RectTransform>());
            }
        }
        parent.gameObject.SetActive(active);
    }
}
