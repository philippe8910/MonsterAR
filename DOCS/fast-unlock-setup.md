# 快速解鎖按鈕設定指南

## 📋 功能概述

新建立的 `FastUnlockButton.cs` 腳本實現一次性解鎖功能：
- **鎖住狀態**：顯示鎖住的鎖頭圖片，可以點擊解鎖
- **解鎖狀態**：顯示開啟的鎖頭圖片，無法再次點擊

## 🔧 Unity 設定步驟

### 1. 找到快速解鎖按鈕
在 Unity 場景中找到原本的快速解鎖按鈕物件

### 2. 替換腳本組件
1. 移除原有的按鈕腳本（如果有的話）
2. 添加 `FastUnlockButton` 腳本到按鈕物件上

### 3. 設定腳本參數
在 `FastUnlockButton` 腳本的 Inspector 中設定：

#### Button Settings
- **Unlock Button**: 拖入按鈕的 Button 組件
- **Button Image**: 拖入按鈕的 Image 組件

#### Lock Sprites
- **Locked Sprite**: 拖入鎖住的鎖頭圖片素材
- **Unlocked Sprite**: 拖入開啟的鎖頭圖片素材

#### Unlock State
- **Is Unlocked**: 預設保持 `false`（鎖住狀態）

### 4. 更新按鈕事件
1. 在按鈕的 `OnClick` 事件中移除原有的 `AllLockoff` 調用
2. 新的腳本會自動處理點擊事件和狀態切換

## 🎯 功能說明

### 主要功能
- **一次性解鎖**：點擊按鈕執行一次性解鎖操作
- **圖片更新**：根據狀態自動更換對應的鎖頭圖片
- **卡片解鎖**：解鎖後會自動解鎖所有卡片，保持解鎖狀態

### 自動重置
- 遊戲開始時自動重置為鎖住狀態
- 一旦解鎖後保持解鎖狀態直到遊戲重新開始

## 🔍 程式碼整合

### 新增的檔案
- `Assets/Script/FastUnlockButton.cs` - 主要功能腳本

### 修改的檔案
- `Assets/Script/ChoseTargetDetected.cs:56-69` - 添加狀態同步
- `Assets/Script/DemonsDetectedManager.cs:36-48` - 添加狀態重置

## 🎨 素材準備

### 鎖頭圖片規格
- **格式**: PNG（建議）
- **尺寸**: 建議與原按鈕圖片相同
- **透明度**: 支援透明背景

### 命名建議
- `lock_closed.png` - 鎖住狀態
- `lock_open.png` - 解鎖狀態

## 🧪 測試步驟

### 基本功能測試
1. 進入卡片選擇場景
2. 點擊快速解鎖按鈕
3. 檢查圖片是否正確切換
4. 確認卡片是否被解鎖

### 狀態重置測試
1. 解鎖後重新進入遊戲
2. 檢查按鈕是否重置為鎖住狀態
3. 確認功能運作正常

## ⚠️ 注意事項

### 相容性
- 確保新腳本與現有的 `ChoseTargetDetected` 系統相容
- 保持原有的 `AllLockoff()` 功能不變

### 效能考量
- 使用 `FindAnyObjectByType<>()` 會有輕微效能影響
- 考慮到專案的緊急性，暫時採用這種方式

### 備份建議
- 在修改前備份原始場景檔案
- 保留原始按鈕設定以防需要回退

## 🚀 快速部署

如果時間緊迫，可以按照以下最小步驟快速部署：

1. 將 `FastUnlockButton.cs` 添加到按鈕物件
2. 設定兩個鎖頭圖片素材
3. 測試基本點擊功能
4. 確認圖片切換正常

## 🔧 問題排除

### 狀態沒有切換
1. **檢查 Console 日誌**：查看是否有 "FastUnlockButton: 按鈕被點擊" 的訊息
2. **如果沒有點擊訊息**：
   - 確認 `FastUnlockButton` 腳本已正確添加到按鈕物件
   - 檢查 Unity Inspector 中的 Button 和 Image 欄位是否正確設定
3. **如果有點擊訊息但狀態沒變**：
   - 使用 `FastUnlockButtonV2.cs` 替代版本
   - 在 Unity Inspector 中手動設定按鈕的 OnClick 事件

### 使用 V2 版本的設定
1. 移除原有的 `FastUnlockButton.cs`
2. 添加 `FastUnlockButtonV2.cs` 到按鈕物件
3. 在按鈕的 OnClick 事件中添加：
   - Object: 拖入按鈕物件本身
   - Function: `FastUnlockButtonV2.OnUnlockButtonClicked()`

### 圖片沒有切換
1. 確認兩個 Sprite 素材都已正確設定
2. 檢查 Console 是否有 "Sprite 為 null" 的錯誤訊息
3. 確認 Button Image 欄位指向正確的 Image 組件

## 🧪 調試資訊

新版本包含詳細的調試資訊，在測試時請查看 Unity Console：
- 初始化完成訊息
- 按鈕點擊事件訊息
- 狀態切換訊息
- 圖片設定訊息

---

*建立時間：2025-07-17*  
*狀態：準備部署（含問題排除）*