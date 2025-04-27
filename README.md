WpfReactiveADS
🔧 Description
Cette application WPF en C# utilise TwinCAT.Ads.Reactive pour établir une communication réactive avec un automate Beckhoff (TwinCAT).

Le programme :

Observe en temps réel une variable PLC (Main.NumRequete) de type ushort.

Affiche les 12 dernières valeurs reçues dans une ListBox.

Génère pour chaque valeur une chaîne ASCII de 1024 caractères et l'écrit dans la variable PLC Main.s1024String.

📚 Fonctionnement général
Connexion au PLC local (AmsNetId.Local, port 851).

Abonnement à une notification cyclique (10 ms) sur la variable Main.NumRequete.

Mise à jour de l'interface WPF en temps réel via le Dispatcher.

Écriture asynchrone dans le PLC pour chaque nouvelle valeur reçue.

🛠️ Technologies utilisées
C#

WPF (.NET Framework / .NET Core)

Beckhoff TwinCAT.Ads (Communication ADS)

TwinCAT.Ads.Reactive (Extensions réactives)

🧩 Exemple de flux
Réception d'un nouveau ushort depuis Main.NumRequete.

Affichage de la valeur dans l'interface WPF.

Génération d'une chaîne de 1024 caractères (Valeur reçue : X...).

Écriture de la chaîne dans la variable Main.s1024String du PLC.

📄 Structure du projet

Fichier	Rôle
MainWindow.xaml	Interface utilisateur
MainWindow.xaml.cs	Logique de communication et observation PLC
⚡ Remarques
Le cycle de notification est configuré à 10 ms pour une haute réactivité.

La gestion de la fermeture (OnClosing) est prévue pour éviter les erreurs de type ObjectDisposedException.

Le projet est extensible pour d'autres types de données PLC si besoin.

🚀 À venir (idées d'amélioration)
Ajouter la possibilité de changer la variable à observer directement depuis l'interface.

Support de l'écriture inverse (WPF → PLC) en réaction à des actions utilisateur.

Gestion plus propre des erreurs ADS et reconnexions automatiques.

Auteur :
Projet personnel pour tester les capacités réactives de la communication TwinCAT ADS en environnement .NET WPF.

