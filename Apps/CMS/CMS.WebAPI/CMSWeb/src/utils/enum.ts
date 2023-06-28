export default {
  /**原料分类 */
  rawMaterialCategoryOptions: [
    { label: '资源性', value: 1 },
    { label: '包装性', value: 2 },
    { label: '辅助性', value: 3 },
    { label: '直投', value: 4 },
    { label: '加工后', value: 5 },
    { label: '特殊', value: 6 },
  ],
  /**储存方式 */
  rawMaterialStorageModelOptions: [
    { label: '冷冻保存冷冻使用', value: 0 },
    { label: '冷冻保存化冻使用', value: 1 },
    { label: '冷藏保存并使用', value: 2 },
    { label: '常温保存并使用', value: 3 },
    { label: '常温保存冷藏使用', value: 4 },
    { label: '常温保存加热使用', value: 5 },
  ],
  /**初始形态 */
  rawMaterialInitialFormOptions: [
    { label: '无', value: 0 },
    { label: '粉状', value: 1 },
    { label: '酱状', value: 2 },
    { label: '固态状', value: 3 },
    { label: '液态状', value: 4 },
    { label: '气态', value: 5 },
  ],
  /**投放设备 */
  rawMaterialPutEquipmentOptions: [
    { label: '粉料仓', value: 1 },
    { label: '液料仓', value: 2 },
    { label: '粉圆机', value: 3 },
    { label: '冰激淋机', value: 4 },
    { label: '红茶仓', value: 5 },
    { label: '绿茶仓', value: 6 },
    { label: '咖啡仓', value: 7 },
    { label: '热奶仓', value: 8 },
    { label: '奶泡仓', value: 9 },
    { label: '热水仓', value: 10 },
    { label: '冰水仓', value: 11 },
    { label: '苏打水仓', value: 12 },
    { label: '制冰机', value: 13 },
    { label: '出杯系统', value: 14 },
    { label: '打印机', value: 15 },
    { label: '红茶胶囊仓', value: 16 },
    { label: '绿茶胶囊仓', value: 17 },
    { label: '咖啡胶囊仓', value: 18 },
    { label: '特殊动作', value: 19 },
    { label: '红绿咖仓', value: 20 },
    { label: '红绿咖胶囊仓', value: 21 },
    { label: '副出茶仓', value: 22 },
    { label: '纯净水仓', value: 23 },
    { label: '污水仓', value: 24 },
  ],
  /**左右手 */
  rawMaterialLeftOrRightOptions: [
    { label: '通用', value: 0 },
    { label: '左手', value: 1 },
    { label: '右手', value: 3 },
  ],
  /**最小包装方式 */
  rawMaterialMiniPackingMethodOptions: [
    { label: '袋', value: 1 },
    { label: '桶', value: 2 },
    { label: '瓶', value: 3 },
    { label: '盒', value: 4 },
    { label: '条', value: 5 },
  ],
  /**投放计量单位 */
  rawMaterialPutUnitOptions: [
    { label: '克(g)', value: 1 },
    { label: '千克(kg)', value: 2 },
    { label: '毫升(ml)', value: 3 },
    { label: '升(L)', value: 4 },
    { label: '个', value: 5 },
  ],
  /**使用限制 */
  rawMaterialLimitOptions: [
    { label: '无限制', value: 1 },
    { label: '冷饮可用', value: 2 },
    { label: '热饮可用', value: 3 },
  ],
  /**配方限制 */
  rawMaterialRecipeLimitOptions: [
    { label: '无限制', value: 0 },
    { label: '标准添加', value: 1 },
    { label: '额外添加', value: 2 },
  ],
};
