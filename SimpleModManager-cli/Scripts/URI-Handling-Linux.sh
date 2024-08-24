sudo su
cp ./Resources/mimeapps.lists /etc/xdg/mimeapps.lists
chmod 644 /etc/xdg/mimeapps.lists

sudo -u $username xdg-mime default SimpleModManager.desktop x-scheme-handler/nxm
