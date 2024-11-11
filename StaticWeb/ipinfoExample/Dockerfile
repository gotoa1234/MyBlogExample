# 使用 Nginx 作為基底映像
FROM nginx:alpine

# 移除預設的 Nginx 頁面
RUN rm -rf /usr/share/nginx/html/*

# 複製本地的 index.html 到 Nginx 網頁根目錄
COPY web/httpsipinfo.ioExample.html /usr/share/nginx/html/index.html

# 曝露 Nginx HTTP 埠
EXPOSE 80

# 啟動 Nginx
CMD ["nginx", "-g", "daemon off;"]