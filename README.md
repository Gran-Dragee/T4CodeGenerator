## T4CodeGenerator

ランタイムT4テンプレートを使用して、JsonSchemaを元にしたスキーマクラスの作成を行うジェネレーター  
テンプレート側でjsonのパースを行うので、追加処理や出力方法のカスタマイズ容易になります  

#### 動作環境

- OSX

#### セットアップ

```
$ brew install mono
```

#### 実行

```
$ cd bin

# 実行後、bin/output以下にres/json_schema以下のスキーマファイルに対応したクラスが生成されます
# 出力ファイル名は、スキーマファイル名.csとして出力されます
$ mono CodeGenerator.exe
```

#### ビルド

```
xbuild CodeGenerator.sln

or

monodevelop等でslnを開いてビルド
```
