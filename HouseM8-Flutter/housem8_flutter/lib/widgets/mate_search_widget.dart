import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/messaging_helper.dart';
import 'package:housem8_flutter/services/mate_search_web_service.dart';
import 'package:housem8_flutter/view_models/mate_view_model.dart';
import 'package:tcard/tcard.dart';

import 'mate_search_card.dart';

class MateSearchWidget extends StatefulWidget {
  final List<MateViewModel> matesList;

  const MateSearchWidget({Key key, this.matesList}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return _MateSearchWidgetState(matesList: matesList);
  }
}

class _MateSearchWidgetState extends State<MateSearchWidget> {
  List<MateViewModel> matesList = List<MateViewModel>();
  TCardController _controller;
  int _index;
  List<Widget> cards;
  double width;
  double height;
  bool isDone;

  _MateSearchWidgetState({this.matesList})
      : this._index = 0,
        this._controller = TCardController();

  @override
  void initState() {
    super.initState();
    isDone = false;
    generateCards();
  }

  @override
  Widget build(BuildContext context) {
    width = MediaQuery.of(context).size.width;
    height = MediaQuery.of(context).size.height;
    if (!isDone) {
      return Scaffold(
        body: Center(
          child: Column(
            children: <Widget>[
              TCard(
                size: Size(width, height / 1.5),
                cards: cards,
                controller: _controller,
                onForward: (index, info) async {
                  _index = index;
                  if (info.direction == SwipDirection.Left) {
                    print("Mate Ignorado!");
                  } else {
                    var chatInstance = MessagingHelper();
                    await chatInstance
                        .createConnection(matesList[_index - 1].userId);
                    await chatInstance
                        .sendMessage("Olá! Gostaria de lhe pedir um serviço!");
                    await chatInstance.stop();
                  }
                  setState(() {});
                },
                onBack: (index) {
                  _index = index;
                  print("Investigar");
                  setState(() {});
                },
                onEnd: () {
                  isDone = true;
                  setState(() {});
                },
              ),
            ],
          ),
        ),
      );
    } else {
      return Scaffold(
          body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Text("Chegou ao fim do feed!",
                style: TextStyle(fontWeight: FontWeight.w700, fontSize: 15)),
            Text("Modique os filtros para ver mais publicações!",
                style: TextStyle(fontWeight: FontWeight.w700, fontSize: 15))
          ],
        ),
      ));
    }
  }

  void generateCards() {
    if (matesList.length > 0) {
      cards = List.generate(
        matesList.length,
        (int index) {
          return Column(children: <Widget>[
            FutureBuilder(
                future: MateSearchWebService()
                    .fetchProfileImage(matesList[index].userId),
                builder: (context, snapshot) {
                  if (snapshot.hasData) {
                    return MateSearchCard(
                      url:
                          "https://10.0.2.2:5001/Uploads/Users/${matesList[index].userId}/${snapshot.data.name}",
                      userName: matesList[index].userName,
                      range: matesList[index].range,
                    );
                  } else {
                    return MateSearchCard(
                      url:
                          "https://www.itl.cat/pngfile/big/91-916122_facebook-blank.jpg",
                      userName: matesList[index].userName,
                      range: matesList[index].range,
                    );
                  }
                }),
          ]);
        },
      );
    } else {
      cards = List<Widget>();
      cards.add(Column(
        children: [
          MateSearchCard(
            url: "https://www.itl.cat/pngfile/big/91-916122_facebook-blank.jpg",
            userName: "",
            range: 0,
          )
        ],
      ));
    }
  }
}
