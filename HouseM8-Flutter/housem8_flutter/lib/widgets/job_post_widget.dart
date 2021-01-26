import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/messaging_helper.dart';
import 'package:housem8_flutter/services/job_post_web_service.dart';
import 'package:housem8_flutter/view_models/job_post_list_view_model.dart';
import 'package:housem8_flutter/view_models/job_post_view_model.dart';
import 'package:housem8_flutter/widgets/job_search_card.dart';
import 'package:housem8_flutter/widgets/offer_dialog.dart';
import 'package:provider/provider.dart';
import 'package:tcard/tcard.dart';

class JobPostWidget extends StatefulWidget {
  final List<JobPostViewModel> jobPosts;

  const JobPostWidget({Key key, this.jobPosts}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return _JobPostWidgetState(jobPosts: jobPosts);
  }
}

class _JobPostWidgetState extends State<JobPostWidget> {
  List<JobPostViewModel> jobPosts = List<JobPostViewModel>();
  TCardController _controller;
  int _index;
  List<Widget> cards;
  double width;
  double height;
  bool isDone;

  _JobPostWidgetState({this.jobPosts})
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
    final vm = Provider.of<JobPostListViewModel>(context, listen: false);
    if (!isDone) {
      return Scaffold(
        body: Center(
          child: Column(
            children: <Widget>[
              TCard(
                size: Size(width, height / 1.5),
                cards: cards,
                controller: _controller,
                onForward: (index, info) {
                  _index = index;
                  if (info.direction == SwipDirection.Left) {
                    vm.ignoreJobPost(_index - 1);
                  } else {
                    showDialog(
                      context: context,
                      builder: (context) => OfferDialog(
                        initialPrice: jobPosts[_index - 1].initialPrice,
                      ),
                    ).then((value) async {
                      if (value != null) {
                        vm.makeOffer(_index - 1, double.parse(value));
                        var chatInstance = MessagingHelper();
                        await chatInstance
                            .createConnection(jobPosts[_index - 1].employerId);
                        await chatInstance.sendMessage(
                            "Olá! Posso realizar o trabalho por $value €.");
                        await chatInstance.stop();
                      }
                    });
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
    if (jobPosts.length > 0) {
      cards = List.generate(
        jobPosts.length,
        (int index) {
          return Column(children: <Widget>[
            FutureBuilder(
                future: JobPostWebService()
                    .fetchMainImage(jobPosts[index].jobPost.id),
                builder: (context, snapshot) {
                  if (snapshot.hasData) {
                    return JobSearchCard(
                      url:
                          "https://10.0.2.2:5001/Uploads/Posts/${jobPosts[index].id}/main/${snapshot.data.name}",
                      title: jobPosts[index].title,
                      range: (() {
                        if (jobPosts[index].range != 0) {
                          return ("Distância: " +
                              jobPosts[index].range.toString() +
                              " KM");
                        } else {
                          return jobPosts[index].address;
                        }
                      }()),
                    );
                  } else {
                    return JobSearchCard(
                      url: "https://www.tibs.org.tw/images/default.jpg",
                      title: jobPosts[index].title,
                      range: (() {
                        if (jobPosts[index].range != 0) {
                          return ("Distância: " +
                              jobPosts[index].range.toString() +
                              " KM");
                        } else {
                          return jobPosts[index].address;
                        }
                      }()),
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
          JobSearchCard(
            url: "https://www.tibs.org.tw/images/default.jpg",
            title: "",
            range: "",
          )
        ],
      ));
    }
  }
}
