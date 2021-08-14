using System.Collections.Generic;
using UnityEngine;

namespace DamageNumbersPro
{
    [CreateAssetMenu(fileName = "New DNP Prefabs Library SO", menuName = "Damage Number Pro/Create DNP Prefabs Library SO")]
    public class DNPPrefabsLibrarySO : Sirenix.OdinInspector.SerializedScriptableObject
    {
        public Dictionary<string, GameObject> DNPPrefabDetails
        {
            get
            {
                if (dnpPrefabDetails == null)
                    dnpPrefabDetails = new Dictionary<string, GameObject>();

                return dnpPrefabDetails;
            }
        }

        public void Add(string prefabName, GameObject dnpPrefab)
        {
            if (!dnpPrefabDetails.ContainsKey(prefabName))
            {
                dnpPrefabDetails.Add(prefabName, dnpPrefab);
            }
            else if (dnpPrefabDetails[prefabName] != dnpPrefab)
            {
                int count = 1;
                string name = prefabName + count;
                while (dnpPrefabDetails.ContainsKey(name))
                {
                    name = prefabName + ++count;

                    if (count > dnpPrefabDetails.Count)
                        break;
                }
                prefabName = name;
                dnpPrefabDetails.Add(prefabName, dnpPrefab);
            }
        }

        private void OnEnable()
        {
            if (dnpPrefabDetails == null)
                dnpPrefabDetails = new Dictionary<string, GameObject>();
        }

        [Sirenix.OdinInspector.DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "Details"), SerializeField]
        private Dictionary<string, GameObject> dnpPrefabDetails = new Dictionary<string, GameObject>();
    }
}