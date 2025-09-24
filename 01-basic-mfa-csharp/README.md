# BasicMfaSample - Entra External ID 基本MFA認証サンプル

Microsoft Entra External ID (CIAM) を使用した認証の基本実装サンプルです。

## 🏗️ アーキテクチャ

### プロジェクト構成
```
BasicMfaSample.sln
├── BasicMfaSample.Models      # データモデル層
├── BasicMfaSample.Services    # ビジネスロジック・認証サービス層
└── BasicMfaSample.Web         # MVC プレゼンテーション層
```

### 技術スタック
- **.NET 9.0**
- **ASP.NET Core MVC**
- **Microsoft.Identity.Web**
- **Microsoft Entra External ID (CIAM)**

## 🚀 セットアップ

### 前提条件
- .NET 9.0 SDK
- Visual Studio 2022 または VS Code
- Microsoft Entra External ID テナント

※テナント上の設定はZennの記事（ https://zenn.dev/articles/e149e813d29dd9 ）の内容で構成されていることを前提とします。

### アプリケーション設定の変更

#### ⚠️ 重要：Program.cs の設定変更
[`BasicMfaSample.Web/appsettings.json`](BasicMfaSample.Web/appsettings.json) の以下の部分を**あなたのAzure環境情報**に書き換えてください：

```json
{
  "AzureAd": {
    "Instance": "https://＜自身のテナント名＞.ciamlogin.com/",      // ← 自身の環境情報で書き換え
    "TenantId": "＜自身のテナントID＞",                             // ← 自身の環境情報で書き換え
    "ClientId": "＜Entraに登録したアプリのクライアントID＞",        // ← 自身の環境情報で書き換え
    "CallbackPath": "/signin-oidc",
    "SignedOutCallbackPath": "/signout-callback-oidc",
    "ClientSecret": "＜Entraに登録したアプリのシークレット値＞"     // ← 自身の環境情報で書き換え
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### 設定値の確認方法

| 設定項目 | 取得場所 |
|---------|---------|
| **テナント名** | Entra admin center → 概要 → テナント情報 |
| **テナントID** | Entra admin center → 概要 → テナント情報 |
| **クライアントID** | アプリの登録 → 概要 → アプリケーション（クライアント）ID |
| **クライアントシークレット** | アプリの登録 → 証明書とシークレット |

### 4. 実行

```bash
cd BasicMfaSample.Web
dotnet run
```

ブラウザで https://localhost:7059 にアクセス

## 📱 MFA認証フロー

### 認証手順
1. **サインイン**ボタンをクリック
2. **Entra External ID**のサインイン画面に遷移
3. **ユーザー登録**または**既存アカウントでサインイン**
4. **MFA認証**（メールによるワンタイムパスワード）
5. **ダッシュボード**画面に遷移

## 🔧 主要機能とコード構成

### 認証フロー
- **サインイン**: OpenID Connect による外部認証
- **MFA**: ユーザーフローで設定された多要素認証
- **サインアウト**: セッション終了とリダイレクト
- **認可**: 認証済みユーザーのみダッシュボードアクセス可能

### アーキテクチャパターン
- **依存性注入**: `IEntraAuthService` によるサービス層分離
- **レイヤー分離**: MVC → Services → Models の3層構造
- **インターフェース分離**: `Services/Interfaces/` フォルダで契約管理

### 主要ファイル

| ファイル | 説明 |
|---------|------|
| [`Program.cs`](BasicMfaSample.Web/Program.cs) | 認証設定・DI設定・ミドルウェア構成 |
| [`Controllers/AccountController.cs`](BasicMfaSample.Web/Controllers/AccountController.cs) | 認証フロー制御（サインイン/サインアウト） |
| [`Controllers/DashboardController.cs`](BasicMfaSample.Web/Controllers/DashboardController.cs) | 認証後のダッシュボード画面 |
| [`Services/Auth/EntraAuthService.cs`](BasicMfaSample.Services/Auth/EntraAuthService.cs) | 認証ビジネスロジック |
| [`Services/Interfaces/IEntraAuthService.cs`](BasicMfaSample.Services/Interfaces/IEntraAuthService.cs) | 認証サービスインターフェース |
## 🔒 セキュリティ考慮事項

- **クライアントシークレット**: 本番環境では Azure Key Vault または環境変数で管理推奨
- **HTTPS**: 本番環境では必須（証明書の適切な設定）
- **セッション管理**: 適切なタイムアウト設定とセキュアクッキー
- **CSRFトークン**: ASP.NET Core で自動対応済み
- **リダイレクト攻撃対策**: 許可されたリダイレクトURLのみ使用

## 🚨 注意事項

### セキュリティ
- **Program.cs にクライアントシークレット等の機密情報を直接記載しないでください**
- 本サンプルは学習・開発用です。本番環境では適切な設定管理を行ってください

### 環境設定
- ポート番号が他のアプリケーションと競合する場合は、`launchSettings.json` で変更してください
- リダイレクトURIを変更した場合は、Entra admin center での登録情報も更新してください

## 📖 関連ドキュメント

- [Microsoft Entra External ID Documentation](https://docs.microsoft.com/en-us/entra/external-id/)
- [Microsoft.Identity.Web](https://docs.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [Entra External ID ユーザーフロー](https://docs.microsoft.com/en-us/entra/external-id/user-flow-overview)

## 📄 License

MIT License - 詳細は [LICENSE](../LICENSE) ファイルを参照してください。