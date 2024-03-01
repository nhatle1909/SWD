import { defineConfig } from "vite";
import react from '@vitejs/plugin-react-swc'
import path from "path";

export default defineConfig({
  root: path.resolve('./'),
  plugins: [react()],
  build: {
    sourcemap: false, // Disable source maps for production builds
  },
  resolve: {
    alias: [{ find: "@", replacement: "/src" }],
  },
  server: {
    port: 3000,
    host: "0.0.0.0",
    watch: {
      usePolling: true,
    },
  },
});
