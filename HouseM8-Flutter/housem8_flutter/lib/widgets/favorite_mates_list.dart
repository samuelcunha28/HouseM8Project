import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/view_models/favorite_mates_list_view_model.dart';
import 'package:housem8_flutter/view_models/favorite_mates_view_model.dart';
import 'package:housem8_flutter/widgets/remove_alert_dialog.dart';
import 'package:provider/provider.dart';

class FavoriteMatesList extends StatefulWidget {
  final List<FavoriteMatesViewModel> favoriteMates;

  FavoriteMatesList([this.favoriteMates]);

  @override
  _FavoriteMatesListState createState() =>
      _FavoriteMatesListState(favoriteMates);
}

class _FavoriteMatesListState extends State<FavoriteMatesList> {
  final List<FavoriteMatesViewModel> favoriteMates;

  _FavoriteMatesListState(this.favoriteMates);

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      itemCount: this.favoriteMates.length,
      itemBuilder: (context, index) {
        final mates = this.favoriteMates[index];

        return Card(
          child: ListTile(
            title: Text(
              mates.favoriteMates.name.split('.').last,
              style: TextStyle(fontSize: 18.0, color: Color(0xFF006064)),
            ),
            trailing: Row(
              mainAxisSize: MainAxisSize.min,
              children: <Widget>[
                IconButton(
                  icon: Icon(Icons.person),
                  tooltip: 'Perfil',
                  color: Color(0xFF006064),
                  iconSize: 35,
                  onPressed: () => print('Perfil do Mate selecionado'),
                ),
                IconButton(
                    icon: Icon(Icons.delete),
                    tooltip: 'Apagar favorito',
                    color: Color(0xFF006064),
                    iconSize: 35,
                    onPressed: () {
                      showDialog(
                          context: context,
                          builder: (context) {
                            return RemoveAlertDialog("Remover Favorito",
                                "Tem a certeza que pretende remover este Mate?");
                          }).then((value) {
                        if (value == "Sim") {
                          final vm = Provider.of<FavoriteMatesListViewModel>(
                              context,
                              listen: false);
                          vm.deleteFavorite(index);
                          setState(() {});
                        }
                      });
                    }),
              ],
            ),
          ),
        );
      },
    );
  }
}
