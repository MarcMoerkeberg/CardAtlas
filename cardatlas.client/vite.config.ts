import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
// import plugin from '@vitejs/plugin-vue';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';
import AutoImport from 'unplugin-auto-import/vite'
import Components from 'unplugin-vue-components/vite'
import Fonts from 'unplugin-fonts/vite'
import Layouts from 'vite-plugin-vue-layouts'
import Vue from '@vitejs/plugin-vue'
import VueRouter from 'unplugin-vue-router/vite'
import Vuetify, { transformAssetUrls } from 'vite-plugin-vuetify'

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "cardatlas.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

// const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
//     env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7047';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
        // plugin()
        VueRouter({
            dts: 'src/typed-router.d.ts',
          }),
          Layouts(),
          AutoImport({
            imports: [
              'vue',
              {
                'vue-router/auto': ['useRoute', 'useRouter'],
              }
            ],
            dts: 'src/auto-imports.d.ts',
            eslintrc: {
              enabled: true,
            },
            vueTemplate: true,
          }),
          Components({
            dts: 'src/components.d.ts',
          }),
          Vue({
            template: { transformAssetUrls },
          }),
          // https://github.com/vuetifyjs/vuetify-loader/tree/master/packages/vite-plugin#readme
          Vuetify({
            autoImport: true,
            styles: {
              configFile: 'src/styles/settings.scss',
            },
          }),
          Fonts({
            google: {
              families: [ {
                name: 'Roboto',
                styles: 'wght@100;300;400;500;700;900',
              }],
            },
          }),
    ],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        // proxy: {
        //     '^/weatherforecast': {
        //         target,
        //         secure: false
        //     }
        // },
        port: 5173,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
