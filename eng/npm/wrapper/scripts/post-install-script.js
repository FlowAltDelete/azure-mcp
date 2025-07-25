const os = require('os');

const platform = os.platform();
const arch = os.arch();

const requiredPackage = `@azure/mcp-${platform}-${arch}`;

try {
  require.resolve(requiredPackage);
} catch (err) {
  console.error(`Missing required package: '${requiredPackage}'`);
  process.exit(1);
}
