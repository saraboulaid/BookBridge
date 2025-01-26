# Bibliothèque Web - Gestion des Livres et Emprunts

## Description

Cette application web permet de gérer une bibliothèque en ligne avec deux types d’utilisateurs :
- **Lecteurs** : peuvent rechercher des livres, consulter leurs informations, emprunter et retourner des livres.
- **Bibliothécaires** : peuvent gérer les livres, suivre les emprunts et les retours, et gérer les utilisateurs.

Les bibliothécaires peuvent effectuer des opérations CRUD (Créer, Lire, Mettre à jour, Supprimer) sur les livres, tandis que les utilisateurs peuvent rechercher des livres et effectuer des emprunts/retours. Le système permet également de gérer les emprunts, de suivre les retours et d’appliquer des pénalités pour les retards.

## Fonctionnalités principales

### 1. **Gestion des livres (CRUD)**
- **Ajouter des livres** : Les bibliothécaires peuvent ajouter un livre avec les informations suivantes :
  - Titre
  - Auteur
  - Éditeur
  - Genre
  - Année de publication
  - Nombre de copies disponibles
- **Modifier des livres** : Les bibliothécaires peuvent mettre à jour les informations d’un livre.
- **Supprimer des livres** : Les bibliothécaires peuvent retirer un livre du catalogue.
- **Consulter les livres** : Les utilisateurs peuvent voir une liste des livres disponibles avec leurs détails.

### 2. **Système de recherche avancée**
- Recherche par :
  - Titre
  - Auteur
  - Genre
  - Année de publication
  - Filtres pour affiner la recherche (par exemple, "Disponibles uniquement")

### 3. **Gestion des emprunts et des retours**
- **Emprunter un livre** : Les utilisateurs peuvent emprunter des livres disponibles. Le système enregistre l'emprunt avec les dates de début et de retour prévues.
- **Retourner un livre** : Les utilisateurs peuvent signaler le retour de livres. Le stock est mis à jour et des pénalités pour retard peuvent être appliquées.

### 4. **Interface d'administration (Bibliothécaires)**
- **Gestion des livres** : Interface dédiée pour ajouter, modifier, supprimer des livres.
- **Suivi des emprunts et des retours** : Voir quels utilisateurs ont emprunté quels livres et leurs dates limites de retour.
- **Gestion des utilisateurs** : Ajouter, modifier ou supprimer des comptes d’utilisateurs.

### 5. **Authentification et autorisation**
- **Authentification** : Les utilisateurs doivent se connecter pour accéder aux fonctionnalités comme l'emprunt ou la gestion.
- **Rôles utilisateurs** :
  - **Lecteurs** : Accès aux fonctionnalités de recherche et d'emprunt/retour des livres.
  - **Bibliothécaires** : Accès à toutes les fonctionnalités administratives.
