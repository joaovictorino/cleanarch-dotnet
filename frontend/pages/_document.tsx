import Document, { Html, Head, Main, NextScript } from 'next/document';

export default class CustomDocument extends Document {
  render() {
    return (
      <Html lang="pt-BR">
        <Head />
        <body>
          <Main />
          <script src="/runtime-config.js" />
          <NextScript />
        </body>
      </Html>
    );
  }
}
