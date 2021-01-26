import 'package:housem8_flutter/models/favorite_mates.dart';

class FavoriteMatesViewModel {
  final FavoriteMates favoriteMates;

  FavoriteMatesViewModel({this.favoriteMates});

  String get name {
    return this.favoriteMates.name;
  }

  String get email {
    return this.favoriteMates.email;
  }
}
