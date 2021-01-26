import 'package:flutter/cupertino.dart';
import 'package:housem8_flutter/models/favorite_mates.dart';
import 'package:housem8_flutter/services/favorite_mates_service.dart';
import 'package:housem8_flutter/view_models/favorite_mates_view_model.dart';

class FavoriteMatesListViewModel extends ChangeNotifier {
  List<FavoriteMatesViewModel> favoriteMates = List<FavoriteMatesViewModel>();

  Future<void> fetchFavoriteMate() async {
    final returned = await FavoriteMatesService().fetchFavoriteMates();
    this.favoriteMates = returned
        .map((favorite) => FavoriteMatesViewModel(favoriteMates: favorite))
        .toList();
    notifyListeners();
  }

  Future<void> deleteFavorite(int index) async {
    FavoriteMates mateRemoved = favoriteMates[index].favoriteMates;
    favoriteMates.removeAt(index);
    notifyListeners();
    await FavoriteMatesService().deleteFavorite(mateRemoved);
  }
}
