const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:5001';

const context = [
  "/api",
  "/Identity",
  "/weatherforecast",
  "/WeatherForecast"
];

const onError = (err, req, resp, target) => {
  console.error(`Proxy error: ${err.message}`);
};

module.exports = function (app) {
  const appProxy = createProxyMiddleware(context, {
    proxyTimeout: 10000,
    target: target,
    onError: onError,
    secure: false,
    //ws: true, // Enable WebSocket proxying
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);

  // Specific route for WebSocket connections
  //app.use(
  //  '/api/chathub',
  //  createProxyMiddleware({
  //    target: target,
  //    changeOrigin: true,
  //    secure: false, // Disable SSL verification
  //    ws: true // Enable WebSocket proxying
  //  })
  //);
};
