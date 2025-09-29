# ClaimsOnTokenSample - Entra External ID ユーザークレーム取得サンプル

Microsoft Entra External ID (CIAM) を使用した認証とユーザークレーム情報取得の実装サンプルです。

## 🏗️ アーキテクチャ

### プロジェクト構成
```
ClaimsOnTokenSample.sln
├── ClaimsOnTokenSample.Models      # データモデル層
├── ClaimsOnTokenSample.Services    # ビジネスロジック・認証サービス層
└── ClaimsOnTokenSample.Web         # MVC プレゼンテーション層
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

#### ⚠️ appsettings.json の設定変更
[`ClaimsOnTokenSample.Web/appsettings.json`](ClaimsOnTokenSample.Web/appsettings.json) の以下の部分を**あなたのAzure環境情報**に書き換えてください：

```json
{
  "AzureAd": {
    "Instance": "https://＜自身のテナント名＞.ciamlogin.com/",      // ← 自身の環境情報で書き換え
    "TenantId": "＜自身のテナントID＞",                             // ← 自身の環境情報で書き換え
    "ClientId": "＜Entraに登録したアプリのクライアントID＞",        // ← 自身の環境情報で書き換え
    "CallbackPath": "/signin-oidc",
    "SignedOutCallbackPath": "/signout-callback-oidc"
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

### 4. 実行

```bash
cd ClaimsOnTokenSample.Web
dotnet run
```

ブラウザで https://localhost:7059 にアクセス

## 📋 ユーザークレーム取得機能

### 主な機能
1. **基本認証**: OpenID Connect による外部認証
2. **クレーム表示**: IDトークンに含まれるユーザー情報をすべて表示
4. **デバッグ機能**: どのクレームが利用可能かリアルタイム確認

### 表示される情報
- **基本情報**: ユーザー名、メールアドレス、表示名、ユーザーID
- **プロファイル情報**: 名、姓、国、都道府県（設定に応じて）
- **カスタム属性**: ユーザーフローで設定したカスタム属性
- **全クレーム**: IDトークンに含まれるすべてのクレーム情報

## 🔧 主要機能とコード構成

### 認証・クレーム取得フロー
- **サインイン**: OpenID Connect による外部認証
- **クレーム解析**: IDトークンからユーザー情報を抽出
- **柔軟な取得**: 複数のクレームタイプパターンに対応
- **デバッグ表示**: 開発者向けクレーム情報の詳細表示

### アーキテクチャパターン
- **依存性注入**: `IEntraAuthService` によるサービス層分離
- **レイヤー分離**: MVC → Services → Models の3層構造
- **インターフェース分離**: `Services/Interfaces/` フォルダで契約管理

### 主要ファイル

| ファイル | 説明 |
|---------|------|
| [`Program.cs`](ClaimsOnTokenSample.Web/Program.cs) | 認証設定・DI設定・ミドルウェア構成 |
| [`Controllers/AccountController.cs`](ClaimsOnTokenSample.Web/Controllers/AccountController.cs) | 認証フロー制御（サインイン/サインアウト） |
| [`Controllers/DashboardController.cs`](ClaimsOnTokenSample.Web/Controllers/DashboardController.cs) | クレーム情報取得・表示 |
| [`Views/Dashboard/Index.cshtml`](ClaimsOnTokenSample.Web/Views/Dashboard/Index.cshtml) | ユーザー情報・クレーム表示画面 |
| [`Services/Auth/EntraAuthService.cs`](ClaimsOnTokenSample.Services/Auth/EntraAuthService.cs) | 認証ビジネスロジック |
| [`Services/Interfaces/IEntraAuthService.cs`](ClaimsOnTokenSample.Services/Interfaces/IEntraAuthService.cs) | 認証サービスインターフェース |

## 🎯 クレーム取得の詳細

### デフォルトで取得できるクレーム
- `sub`: ユーザーID（必須）
- `name`: 表示名またはメールアドレス
- `email`: メールアドレス
- `preferred_username`: ログインID

### 追加設定で取得できるクレーム
ユーザーフローの「返す属性」設定が必要：
- `given_name`: 名
- `family_name`: 姓
- `country`: 国
- `state`: 都道府県
- `extension_*`: カスタム属性

### クレーム取得の仕組み
1. **IDトークン**: 認証時に返される生のクレーム情報
2. **User.Identity**: ASP.NET Core によるクレーム解釈結果
3. **Controller**: 複数パターンでの堅牢な情報取得
4. **View**: ユーザーフレンドリーな表示

## 🔒 セキュリティ考慮事項

- **クライアントシークレット**: User Secrets で管理（本番環境では Azure Key Vault 推奨）
- **HTTPS**: 本番環境では必須（証明書の適切な設定）
- **セッション管理**: 適切なタイムアウト設定とセキュアクッキー
- **CSRFトークン**: ASP.NET Core で自動対応済み
- **リダイレクト攻撃対策**: 許可されたリダイレクトURLのみ使用
- **クレーム情報**: 必要最小限の情報のみ取得・表示

## 🚨 注意事項

### セキュリティ
- **Client Secret は User Secrets または環境変数で管理してください**
- **本サンプルは学習・開発用です**。本番環境では適切な設定管理を行ってください
- **デバッグ機能**: 本番環境ではクレーム詳細表示を無効化することを推奨

### 環境設定
- ポート番号が他のアプリケーションと競合する場合は、`launchSettings.json` で変更してください
- リダイレクトURIを変更した場合は、Entra admin center での登録情報も更新してください
- **管理者同意**: Entra External ID では管理者による事前同意が必要です

### クレーム取得
- **ユーザーフロー設定**: 追加属性は「返す属性」にチェックが必要
- **カスタム属性**: `extension_{ClientId}_{AttributeName}` の形式
- **互換性**: 複数のクレームタイプに対応した実装を推奨

## 📖 関連ドキュメント

- [Microsoft Entra External ID Documentation](https://docs.microsoft.com/en-us/entra/external-id/)
- [Microsoft.Identity.Web](https://docs.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [OpenID Connect Claims](https://openid.net/specs/openid-connect-core-1_0.html#Claims)
- [Entra External ID ユーザーフロー](https://docs.microsoft.com/en-us/entra/external-id/user-flow-overview)

## 📄 License

MIT License - 詳細は [LICENSE](../LICENSE) ファイルを参照してください。