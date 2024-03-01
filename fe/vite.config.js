import { defineConfig } from "vite";
import react from '@vitejs/plugin-react-swc'
import path from "path";

export default defineConfig({
  root: path.resolve('./'),
  plugins: [react({
    input: [
      '/src/assets/js/jquery.min.js',
      '/src/assets/js/jquery-migrate-3.0.1.min.js',
      '/src/assets/js/popper.min.js',
      '/src/assets/js/bootstrap.min.js',
      '/src/assets/js/jquery.easing.1.3.js',
      '/src/assets/js/jquery.waypoints.min.js',
      '/src/assets/js/jquery.stellar.min.js',
      '/src/assets/js/owl.carousel.min.js',
      '/src/assets/js/jquery.magnific-popup.min.js',
      '/src/assets/js/aos.js',
      '/src/assets/js/jquery.animateNumber.min.js',
      '/src/assets/js/bootstrap-datepicker.js',
      '/src/assets/js/jquery.timepicker.min.js',
      '/src/assets/js/scrollax.min.js',
      '/src/assets/js/google-map.js',
      '/src/assets/js/main.js'
  ],
  refresh: true,
  }) 
  ],
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
